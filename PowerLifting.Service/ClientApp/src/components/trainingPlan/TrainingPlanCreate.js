import React from "react";
import { connect } from "react-redux";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Container, Button } from "reactstrap";
import { createTrainingPlan } from "../../stores/trainingPlanStore/trainingPlanActions";
import { Locale } from "../../common/Localization";
import WithRouter from "../../common/extensions/WithRouter";

const mapStateToProps = store => {
  return {
    planId: store.planId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    createTrainingPlan: (creationDate) => createTrainingPlan(creationDate, dispatch)
  }
}

class TrainingPlanCreate extends React.Component {

  state = {
    date: new Date(),
  }

  onDateChange = date => this.setState({ date: date });

  onPlanCreate = async () => {
    await this.props.createTrainingPlan(this.state.date);
    this.props.navigate("/trainingDaysSetup");
  }

  render() {
    return (
      <>
        <h1>Создание плана тренировок</h1>
        <br />

        <Container fluid>
          <p>Выберите дату начала тренировок</p>
          <Calendar onChange={this.onDateChange} value={this.state.date} locale={Locale} />

          <Button style={{ marginTop: '25px' }} onClick={() => this.onPlanCreate()}>Создать</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(TrainingPlanCreate))