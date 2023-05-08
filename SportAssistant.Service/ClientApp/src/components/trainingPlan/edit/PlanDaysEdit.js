import React from "react";
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, DeleteAsync } from "../../../common/ApiActions";
import { ErrorPanel, Tooltip } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal, Locale } from "../../../common/LocalActions";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import '../../../styling/Common.css';

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class PlanDaysEdit extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      plan: [],
      backUrl: '',
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var plan = await GetAsync(`/trainingPlan/${this.props.params.planId}`);
    var url = plan.isMyPlan ? `/plansList` : `/groupUser/${plan.userId}`;

    this.setState({ plan: plan, backUrl: url });
  }

  onSetExercises = (dayId) => { this.props.navigate(`/editPlanExercises/${this.props.params.planId}/${dayId}`); }

  onConfirmDelete = async () => {
    try {
      await DeleteAsync(`/trainingPlan/${this.props.params.planId}`);
      this.props.navigate(this.state.backUrl);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onDeletePlan = async (lngStr) => {
    var modalInfo = {
      isVisible: true,
      headerText: lngStr('appSetup.modal.confirm'),
      buttons: [{ name: lngStr('general.actions.confirm'), onClick: this.onConfirmDelete, color: "success" }],
      body: () => { return (<p>{lngStr('training.plan.confirmDeletion')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    const lngStr = this.props.lngStr;
    var days = this.state.plan?.trainingDays ?? [];
    const placeHolder = lngStr('general.common.notSet');
    var ownerName = this.state.plan?.owner?.name;

    return (
      <>
        <h4>{lngStr('training.plannedDays')}{ownerName ? ' (' + ownerName + ')' : ''}</h4>
        <br />
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={3} md={{ offset: 4 }}><strong>{lngStr('training.exercise.setDays')}</strong></Col>
        </Row>
        <br />
        <Container fluid>
          <Row>
            <Col>{days.length >= 1 ? this.dayPanel(days[0]) : placeHolder}</Col>
            <Col>{days.length >= 2 ? this.dayPanel(days[1]) : placeHolder}</Col>
            <Col>{days.length >= 3 ? this.dayPanel(days[2]) : placeHolder}</Col>
          </Row>
          <Row style={{ marginTop: '100px' }}>
            <Col>{days.length >= 4 ? this.dayPanel(days[3]) : placeHolder}</Col>
            <Col>{days.length >= 5 ? this.dayPanel(days[4]) : placeHolder}</Col>
            <Col>{days.length >= 6 ? this.dayPanel(days[5]) : placeHolder}</Col>
          </Row>
          <Row style={{ marginTop: '100px', marginBottom: '50px' }}>
            <Col>{days.length >= 7 ? this.dayPanel(days[6]) : placeHolder}</Col>

            <Col>
              <div><strong>{lngStr('training.exercise.total')}</strong></div>
              {this.countersPanel(this.state.plan?.counters, lngStr)}
            </Col>

            <Col>{this.buttonPanel(lngStr)}</Col>
          </Row>
        </Container>
      </>
    );
  }

  dayPanel(day) {
    const lngStr = this.props.lngStr;
    var dateName = new Date(day.activityDate).toLocaleString(Locale, { weekday: 'long' });
    dateName = dateName.charAt(0).toUpperCase() + dateName.slice(1);
    var dayCounters = day?.counters;

    return (
      <>
        <Row>
          <Col className="text-center">
            <div><strong>{dateName}</strong></div>
            <div>{DateToLocal(day.activityDate)}</div>
          </Col>
          <Col className="daysButtonSection" >
            <Button id={dateName + 'btnAssign'} color="primary" outline style={{ width: '40px', marginRight: '10px' }} onClick={() => this.onSetExercises(day.id)} >{' + '}</Button>
            <Button id={dateName + 'btnTransfer'} color="primary" outline style={{ width: '40px' }} disabled={dayCounters.length == 0} onClick={() => this.props.navigate(`/movePlanDay/${this.props.params.planId}/${day.id}`)} >{' - '}</Button>
          </Col>
        </Row>
        <hr style={{ width: '60%', paddingTop: "2px" }} />
        <Row>
          <Col>{this.countersPanel(dayCounters, lngStr)}</Col>
        </Row>

        <Tooltip text={lngStr('training.exercise.assign')} tooltipTargetId={dateName + 'btnAssign'} />
        <Tooltip text={lngStr('training.exercise.transfer')} tooltipTargetId={dateName + 'btnTransfer'} />
      </>
    );
  }

  countersPanel(counters, lngStr) {
    var fontSize = "0.85rem";

    if (!counters) {
      return (
        <span style={{ fontSize: fontSize }}>{lngStr('training.exercise.nothingAssigned')}</span>
      );
    }

    return (
      <>
        <Row style={{ fontSize: fontSize }}>
          <Col>{lngStr('training.entity.liftCounter')}</Col>
          <Col>{counters.liftCounterSum}</Col>
        </Row>
        <Row style={{ fontSize: fontSize }}>
          <Col>{lngStr('training.entity.weightLoad')}</Col>
          <Col>{counters.weightLoadSum}</Col>
        </Row>
        <Row style={{ fontSize: fontSize }}>
          <Col>{lngStr('training.entity.intensity')}</Col>
          <Col>{counters.intensitySum}</Col>
        </Row>
      </>
    );
  }

  buttonPanel(lngStr) {
    return (
      <Col>
        <Button color="primary" onClick={async () => this.props.navigate(this.state.backUrl)}>{lngStr('training.plan.finishAssignment')}</Button>
        <p></p>
        <Button color="primary" outline onClick={async () => this.onDeletePlan(lngStr)}>{lngStr('training.plan.delete')}</Button>
      </Col>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(PlanDaysEdit))