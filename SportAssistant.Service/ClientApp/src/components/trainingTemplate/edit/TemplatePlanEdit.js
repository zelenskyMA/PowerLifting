import React from "react";
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputText } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { changeModalVisibility } from "../../../stores/appStore/appActions";

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class TemplatePlanEdit extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      plan: Object,
      days: [],
      typeCounters: [],
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var planData = await GetAsync(`/templatePlan/get?Id=${this.props.params.id}`);
    this.setState({ plan: planData, days: planData.trainingDays, typeCounters: planData.typeCountersSum  });
  }

  onPlanChange = (propName, value) => { this.setState(prevState => ({ error: '', plan: { ...prevState.plan, [propName]: value } })); }

  onSetExercises = (dayId) => { this.props.navigate(`/editTemplateExercises/${this.props.params.id}/${dayId}`); }

  onConfirmPlan = async () => {
    var setId = await PostAsync("/templatePlan/update", this.state.plan);
    this.props.navigate(`/templateSet/${setId}`);
  }

  onConfirmDelete = async () => {
    try {
      var setId = await PostAsync("/templatePlan/delete", { id: this.props.params.id });
      this.props.navigate(`/templateSet/${setId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onDeletePlan = async (lngStr) => {
    var modalInfo = {
      isVisible: true,
      headerText: lngStr('modal.confirm'),
      buttons: [{ name: lngStr('button.confirm'), onClick: this.onConfirmDelete, color: "success" }],
      body: () => { return (<p>{lngStr('training.confirmDeletion')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    const lngStr = this.props.lngStr;

    const days = this.state.days;
    const placeHolder = lngStr('common.notSet');

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceBottom">
          <Col xs={8} md={{ offset: 2 }}>
            <InputText label={lngStr('common.name')} propName="name" onChange={this.onPlanChange} initialValue={this.state.plan.name} />
          </Col>
        </Row>

        <Row>
          <Col xs={3} md={{ offset: 4 }}><strong>{lngStr('training.setExercises')}</strong></Col>
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
              <div><strong>{lngStr('training.totalExercises')}</strong></div>
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

    return (
      <Container fluid>
        <Row>
          <Col className="text-center">
            <div style={{ paddingTop: '7px', paddingLeft: '50px' }}><strong>{`${lngStr('common.day')} ${day.dayNumber}`}</strong></div>
          </Col>
          <Col style={{ paddingTop: '7px', paddingRight: '45px' }} >
            <Button color="primary" outline onClick={() => this.onSetExercises(day.id)} >{' + '}</Button>
          </Col>
        </Row>
        <hr style={{ width: '60%', paddingTop: "2px" }} />
        <Row>
          <Col>{this.countersPanel(day.exerciseTypeCounters, lngStr)}</Col>
        </Row>
      </Container>
    );
  }

  countersPanel(counters, lngStr) {
    var fontSize = "0.85rem";

    if (counters.length == 0) {
      return (
        <span style={{ fontSize: fontSize }}>{lngStr('training.noAssignedExercises')}</span>
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
        <Button color="primary" onClick={async () => this.onConfirmPlan()}>{lngStr('training.confirmTemplate')}</Button>
        <p></p>
        <Button color="primary" outline onClick={async () => this.onDeletePlan(lngStr)}>{lngStr('training.deleteTemplate')}</Button>
      </Col>
    );
  }

}

export default WithRouter(connect(null, mapDispatchToProps)(TemplatePlanEdit))