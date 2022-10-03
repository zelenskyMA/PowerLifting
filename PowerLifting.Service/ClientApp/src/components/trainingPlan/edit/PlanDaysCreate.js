import React from "react";
import { connect } from "react-redux";
import { Container, Button, Row, Col } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { setGroupUserId } from "../../../stores/coachingStore/coachActions";
import { Locale, DateToLocal } from "../../../common/Localization";
import WithRouter from "../../../common/extensions/WithRouter";

const mapStateToProps = store => {
  return {
    planId: store.trainingPlan.planId,
    groupUserId: store.coach.groupUserId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    setGroupUserId: (userId) => setGroupUserId(userId, dispatch)
  }
}

class PlanDaysCreate extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      plannedDays: [],
      typeCounters: []
    };
  }

  componentDidMount() { this.getTrainingPlan(); }

  getTrainingPlan = async () => {
    var plan = await GetAsync(`/trainingPlan/get?Id=${this.props.planId}`);
    this.setState({ plannedDays: plan.trainingDays, typeCounters: plan.typeCountersSum });
  }

  onSetExercises = (dayId) => { this.props.navigate(`/createPlanExercises/${dayId}`); }


  render() {
    const days = this.state.plannedDays;
    const placeHolder = "Не задано";

    return (
      <>
        <h3>Запланированные дни тренировок</h3>
        <br />
        <Row>
          <Col xs={3} md={{ offset: 4 }}><strong>Назначьте упражнения на дни недели.</strong></Col>
          <Col>{this.confirmButtonPanel()}</Col>
        </Row>
        <br />
        <Container fluid>
          <Row>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[0])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[1])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[2])}</Col>
          </Row>
          <Row style={{ marginTop: '100px' }}>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[3])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[4])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[5])}</Col>
          </Row>
          <Row style={{ marginTop: '100px', marginBottom: '50px' }}>
            <Col>{days.length === 0 ? placeHolder : this.plannedDayPanel(days[6])}</Col>

            <Col>
              <div><strong>Всего упражнений</strong></div>
              {this.exerciseCountersPanel(this.state.typeCounters)}
            </Col>

            <Col></Col>
          </Row>
        </Container>
      </>
    );
  }

  plannedDayPanel(day) {
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
            <Button color="primary" outline onClick={() => this.onSetExercises(day.id)} >{' + '}</Button>
          </Col>
        </Row>
        <hr style={{ width: '60%', paddingTop: "2px" }} />
        <Row>
          <Col>{this.exerciseCountersPanel(day.exerciseTypeCounters)}</Col>
        </Row>
      </Container>
    );
  }

  exerciseCountersPanel(counters) {
    var fontSize = "0.85rem";

    if (counters.length == 0) {
      return (
        <span style={{ fontSize: fontSize }}>Нет назначенных тренировок</span>
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

  confirmButtonPanel() {
    if (this.state.typeCounters.length == 0) {
      return (<></>);
    }

    var url = this.props.groupUserId ? `/groupUser/${this.props.groupUserId}` : "/plansList";

    return (<Col>
      <Button color="primary" onClick={async () => this.props.navigate(url)}>Завершить</Button>
    </Col>);
  }

}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(PlanDaysCreate))