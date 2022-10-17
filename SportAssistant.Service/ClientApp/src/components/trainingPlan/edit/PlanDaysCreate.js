import React from "react";
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal, Locale } from "../../../common/Localization";
import { setGroupUserId } from "../../../stores/coachingStore/coachActions";

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

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var plan = await GetAsync(`/trainingPlan/get?Id=${this.props.planId}`);
    this.setState({ plannedDays: plan.trainingDays, typeCounters: plan.typeCountersSum });
  }

  onSetExercises = (dayId) => { this.props.navigate(`/createPlanExercises/${dayId}`); }

  onDeletePlan = (url) => {

    this.props.navigate(url);
  }

  render() {
    const days = this.state.plannedDays;
    const placeHolder = "Не задано";

    return (
      <>
        <h3>Запланированные дни тренировок</h3>
        <br />
        <Row>
          <Col xs={3} md={{ offset: 4 }}><strong>Назначьте упражнения на дни недели.</strong></Col>
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
              <div><strong>Всего упражнений</strong></div>
              {this.countersPanel(this.state.typeCounters)}
            </Col>

            <Col>{this.buttonPanel()}</Col>
          </Row>
        </Container>
      </>
    );
  }

  dayPanel(day) {
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
          <Col>{this.countersPanel(day.exerciseTypeCounters)}</Col>
        </Row>
      </Container>
    );
  }

  countersPanel(counters) {
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

  buttonPanel() {
    if (this.state.typeCounters.length == 0) { return (<></>); }

    var url = this.props.groupUserId ? `/groupUser/${this.props.groupUserId}` : "/plansList";

    return (
      <Col>
        <Button color="primary" onClick={async () => this.props.navigate(url)}>Завершить назначение плана</Button>
        <p></p>
        <Button color="primary" outline onClick={async () => this.onDeletePlan(url)}>Удалить план</Button>
      </Col>
    );
  }

}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(PlanDaysCreate))