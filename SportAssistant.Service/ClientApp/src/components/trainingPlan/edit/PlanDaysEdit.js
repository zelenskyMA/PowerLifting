import React from "react";
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, DeleteAsync } from "../../../common/ApiActions";
import { ErrorPanel, Tooltip } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal, Locale } from "../../../common/LocalActions";
import { changeModalVisibility } from "../../../stores/appStore/appActions";

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class PlanDaysEdit extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      plannedDays: [],
      typeCounters: [],
      backUrl: '',
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var plan = await GetAsync(`/trainingPlan/${this.props.params.planId}`);
    var url = plan.isMyPlan ? `/plansList` : `/groupUser/${plan.userId}`;

    this.setState({ plannedDays: plan.trainingDays, typeCounters: plan.typeCountersSum, backUrl: url });
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
    const days = this.state.plannedDays;
    const placeHolder = lngStr('general.common.notSet');

    return (
      <>
        <h4>{lngStr('training.plannedDays')}</h4>
        <br />
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={3} md={{ offset: 4 }}><strong>{lngStr('training.exercise.setDays')}</strong></Col>
        </Row>
        <br />
        <Container fluid>
          <Row>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[0])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[1])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[2])}</Col>
          </Row>
          <Row style={{ marginTop: '100px' }}>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[3])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[4])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[5])}</Col>
          </Row>
          <Row style={{ marginTop: '100px', marginBottom: '50px' }}>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[6])}</Col>

            <Col>
              <div><strong>{lngStr('training.exercise.total')}</strong></div>
              {this.countersPanel(this.state.typeCounters, lngStr)}
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

    return (
      <Container fluid>
        <Row>
          <Col className="text-center">
            <div><strong>{dateName}</strong></div>
            <div>{DateToLocal(day.activityDate)}</div>
          </Col>
          <Col style={{ paddingTop: '7px' }} >
            <Button id={dateName + 'btnAssign'} color="primary" outline style={{ width: '40px', marginRight: '10px' }} onClick={() => this.onSetExercises(day.id)} >{' + '}</Button>
            <Button id={dateName + 'btnTransfer'} color="primary" outline style={{ width: '40px' }} disabled={day.exerciseTypeCounters.length == 0} onClick={() => this.props.navigate(`/movePlanDay/${this.props.params.planId}/${day.id}`)} >{' - '}</Button>
          </Col>
        </Row>
        <hr style={{ width: '60%', paddingTop: "2px" }} />
        <Row>
          <Col>{this.countersPanel(day.exerciseTypeCounters, lngStr)}</Col>
        </Row>

        <Tooltip text={lngStr('training.exercise.assign')} tooltipTargetId={dateName + 'btnAssign'} />
        <Tooltip text={lngStr('training.exercise.transfer')} tooltipTargetId={dateName + 'btnTransfer'} />
      </Container>
    );
  }

  countersPanel(counters, lngStr) {
    var fontSize = "0.85rem";

    if (counters.length == 0) {
      return (
        <span style={{ fontSize: fontSize }}>{lngStr('training.exercise.nothingAssigned')}</span>
      );
    }

    return (
      <>
        {counters.map((item, i) =>
          <Row key={i} style={{ fontSize: fontSize }}>
            <Col>{item.name}</Col>
            <Col>{item.value}</Col>
          </Row>
        )}
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