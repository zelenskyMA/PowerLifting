import React from "react";
import { connect } from "react-redux"
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Container } from "reactstrap";
import { GoToButton } from "../../common/Navigation";
import { createTrainingPlan } from "../../stores/trainingPlanStore/trainingPlanActions";
import { Locale } from "../../common/Localization";

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
  onPlanCreate = async () => { await this.props.createTrainingPlan(this.state.date); }

  render() {
    return (
      <>
        <h1>Создание плана тренировок</h1>
        <br />

        <Container fluid>
          <p>Выберите дату начала тренировок</p>
          <Calendar onChange={this.onDateChange} value={this.state.date} locale={Locale} />
          <br />
          <GoToButton url="/trainingDaysSetup" beforeNavigate={this.onPlanCreate} name="Создать" />
        </Container>
      </>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(TrainingPlanCreate)