import React from "react";
import { connect } from "react-redux"
import { Container, Button, Row, Col } from "reactstrap";
import { GetAsync } from "../../common/ApiActions"


const mapStateToProps = store => {
  return {
    planId: store.planId,
    plannedDays: null
  }
}

class TrainingDaysSetup extends React.Component {
  constructor() { super(); }

  componentDidMount() {
    this.getTrainingPlan();
  }

  async getTrainingPlan() {
    var data = await GetAsync(`trainingPlan/get?Id${this.state.planId}`);
    this.setState({ plannedDays: data.trainingDays });
  }

  render() {
    return (
      <>
        <h1>Запланированные дни тренировок</h1>
        <Container fluid>
          <Row>
            <Col>{this.plannedDayPanel(this.state.plannedDays[0])}</Col>
            <Col>{this.plannedDayPanel(this.state.plannedDays[1])}</Col>
            <Col>{this.plannedDayPanel(this.state.plannedDays[2])}</Col>
          </Row>
          <br />
          <Row>
            <Col>{this.plannedDayPanel(this.state.plannedDays[3])}</Col>
            <Col>{this.plannedDayPanel(this.state.plannedDays[4])}</Col>
            <Col>{this.plannedDayPanel(this.state.plannedDays[5])}</Col>
          </Row>
          <br />
          <Row>
            <Col>{this.plannedDayPanel(this.state.plannedDays[6])}</Col>
            <Col></Col>
            <Col></Col>
          </Row>
          <Button>Подтвердить</Button>
        </Container>
      </>
    );
  }

  plannedDayPanel({ day }) {
    const dateName = day.activityDate.toLocaleString(window.navigator.language, { weekday: 'short' });
    const exercises = day.exercises.length > 0 ? 'Они есть' : 'Нет назначенных тренировок';

    return (
      <Container fluid>
        <Row>
          <Col><p>{dateName}</p>{day.activityDate}</Col>
          <Col>{exercises}</Col>
        </Row>
        <Row>
          <Col>
            <Button>' + '</Button>
          </Col>
        </Row>
      </Container>
    );
  }
}

export default connect(mapStateToProps, null)(TrainingDaysSetup)