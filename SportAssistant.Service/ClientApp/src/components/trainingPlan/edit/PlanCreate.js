import React from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Button, Container } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { ErrorPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToUtc, Locale } from "../../../common/LocalActions";
import '../../../styling/Common.css';

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
      const planId = await PostAsync("trainingPlan/create", { creationDate: DateToUtc(this.state.date), userId: this.props.params.groupUserId });
      this.props.navigate(`/editPlanDays/${planId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4>{lngStr('training.createTrainingPlan')}</h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Container className="spaceTop" fluid>
          <p>{lngStr('training.selectTrainingStart')}</p>
          <Calendar onChange={this.onDateChange} value={this.state.date} locale={Locale} />

          <Button color="primary" className="spaceTop" onClick={() => this.onPlanCreate()}>{lngStr('button.create')}</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(PlanCreate)