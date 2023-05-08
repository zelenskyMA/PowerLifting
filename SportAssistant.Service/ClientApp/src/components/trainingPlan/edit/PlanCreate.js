import React from "react";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Button, Container, Col, Row } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputNumber } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToUtc, Locale } from "../../../common/LocalActions";
import '../../../styling/Common.css';

class PlanCreate extends React.Component {
  constructor() {
    super();

    this.state = {
      date: new Date(),
      daysCount: 7,
      error: ''
    }
  }

  onDateChange = date => this.setState({ date: date });

  onValueChange = (propName, value) => this.setState({ daysCount: value });

  onPlanCreate = async () => {
    try {
      var request = {
        creationDate: DateToUtc(this.state.date),
        userId: this.props.params.groupUserId,
        daysCount: this.state.daysCount
      };

      const planId = await PostAsync("trainingPlan", request);
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
        <h4>{lngStr('training.plan.create')}</h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Container className="spaceTop" fluid>
          <p>{lngStr('training.selectStartDate')}</p>
          <Calendar onChange={this.onDateChange} value={this.state.date} locale={Locale} />

          <Row className="spaceTop">
            <Col xs={5}>
              <InputNumber label={lngStr('training.plan.daysCountSelection') + ':'} onChange={this.onValueChange} initialValue={this.state.daysCount} />
            </Col>
          </Row>

          <Button color="primary" className="spaceTop" onClick={() => this.onPlanCreate()}>{lngStr('general.actions.create')}</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(PlanCreate)