import React from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { connect } from "react-redux";
import { Button, Container } from "reactstrap";
import { ErrorPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { Locale } from "../../../common/Localization";
import { createTrainingPlan } from "../../../stores/trainingPlanStore/planActions";

const mapStateToProps = store => {
  return {
    planId: store.trainingPlan.planId,
    groupUserId: store.coach.groupUserId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    createTrainingPlan: (request) => createTrainingPlan(request, dispatch)
  }
}

class PlanCreate extends React.Component {

  state = {
    date: new Date(),
    error: ''
  }

  onDateChange = date => this.setState({ date: date });

  onPlanCreate = async () => {
    try {
      var utcDate = new Date(this.state.date.getTime() - this.state.date.getTimezoneOffset() * 60 * 1000);
      var request = { creationDate: utcDate, userId: this.props.groupUserId };

      await this.props.createTrainingPlan(request);
      this.props.navigate("/createPlanDays");
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    return (
      <>
        <h3>Создание плана тренировок</h3>
        <ErrorPanel errorMessage={this.state.error} />

        <Container style={{ marginTop: '25px' }} fluid>
          <p>Выберите дату начала тренировок</p>
          <Calendar onChange={this.onDateChange} value={this.state.date} locale={Locale} />

          <Button color="primary" style={{ marginTop: '25px' }} onClick={() => this.onPlanCreate()}>Создать</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(PlanCreate))