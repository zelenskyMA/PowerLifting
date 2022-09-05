import React from "react";
import { connect } from "react-redux";
import { Container, Button, Row, Col } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import { Locale } from "../../common/Localization";


const mapStateToProps = store => {
  return {
    planId: store.trainingPlan.planId,
  }
}

class TrainingDaysSetup extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      plannedDays: []
    };
  }

  componentDidMount() {
    this.getTrainingPlan();
  }

  getTrainingPlan = async () => {
    var data = await GetAsync(`trainingPlan/get?Id=${this.props.planId}`);
    this.setState({ plannedDays: data.trainingDays });
  }

  render() {
    const days = this.state.plannedDays;
    const placeHolder = "Не задано";

    return (
      <>
        <h1>Запланированные дни тренировок</h1>
        <p>Назначьте упражнения на дни недели.</p>
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
            <Col></Col>
            <Col></Col>
          </Row>
          <Button>Подтвердить</Button>
        </Container>
      </>
    );
  }

  plannedDayPanel(day) {
    var dateValue = new Date(day.activityDate);

    var dateName = dateValue.toLocaleString(Locale, { weekday: 'long' });
    dateName = dateName.charAt(0).toUpperCase() + dateName.slice(1);

    var dateView = dateValue.toLocaleString(Locale, { dateStyle: "medium" });

    const exercises = day.exercises.length > 0 ? 'Они есть' : 'Нет назначенных тренировок';

    return (
      <Container fluid>
        <Row>
          <Col className="text-center">
            <div><strong>{dateName}</strong></div>
            <div>{dateView}</div>
          </Col>
          <Col style={{ paddingTop: '7px' }} >
            <Button color="primary" outline>{' + '}</Button>
          </Col>
        </Row>
        <hr style={{ width: '60%', paddingTop: "2px" }} />
        <Row>
          <Col>{exercises}</Col>
        </Row>
      </Container>
    );
  }
}

export default connect(mapStateToProps, null)(TrainingDaysSetup)