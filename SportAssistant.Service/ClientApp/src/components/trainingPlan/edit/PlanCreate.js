import React from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { connect } from "react-redux";
import { PostAsync } from "../../../common/ApiActions";
import { Button, Container } from "reactstrap";
import { ErrorPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { Locale } from "../../../common/Localization";
import '../../../styling/Common.css';

const mapStateToProps = store => {
  return {
    groupUserId: store.coach.groupUserId,
  }
}

class PlanCreate extends React.Component {
  constructor() {
    super();

    this.state = {
      date: new Date(),
      error: ''
    }
  }

  onDateChange = date => this.setState({ date: date });

  onPlanCreate = async () => {
    try {
      var utcDate = new Date(this.state.date.getTime() - this.state.date.getTimezoneOffset() * 60 * 1000);

      const planId = await PostAsync("trainingPlan/create", { creationDate: utcDate, userId: this.props.groupUserId });
      this.props.navigate(`/editPlanDays/${planId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    return (
      <>
        <h4>Создание плана тренировок</h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Container className="spaceTop" fluid>
          <p>Выберите дату начала тренировок</p>
          <Calendar onChange={this.onDateChange} value={this.state.date} locale={Locale} />

          <Button color="primary" style={{ marginTop: '25px' }} onClick={() => this.onPlanCreate()}>Создать</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(PlanCreate))