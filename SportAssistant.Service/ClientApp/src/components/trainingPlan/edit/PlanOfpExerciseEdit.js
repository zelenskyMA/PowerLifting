import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PutAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputTextArea, LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

const mapStateToProps = store => {
  return {
    appSettings: store.app.settings,
  }
}

/* ОФП упражнения с различными способами планирования (минуты, подходы и т.п.) */
class PlanOfpExerciseEdit extends Component {
  constructor() {
    super();

    this.state = {
      planExercise: Object,
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var data = await GetAsync(`/planExercise/${this.props.params.id}`);

    this.setState({ planExercise: data, loading: false });
  }

  confirmAsync = async () => {
    try {
      var planExercise = this.state.planExercise;

      await PutAsync('/planExercise', { planExercise: planExercise });
      this.props.navigate(`/editPlanDay/${this.props.params.planId}/${planExercise.planDayId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onExerciseChange = (propName, value) => { this.setState(prevState => ({ error: '', planExercise: { ...prevState.planExercise, [propName]: value } })); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4><i>{this.state.planExercise.exercise.name}</i></h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={5}>
            <InputTextArea onChange={this.onExerciseChange} propName="comments" cols={85}
              label={lngStr('training.leaveComment')} initialValue={this.state.planExercise.comments} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={5}>
            <div>{lngStr('training.exercise.target') + ':'}</div>
            <InputTextArea onChange={this.onExerciseChange} propName="extPlanData" cols={85}
              label={lngStr('training.externalTraining')} initialValue={this.state.planExercise.extPlanData} />
          </Col>
        </Row>

        <Button className="spaceTop" color="primary" onClick={() => this.confirmAsync()}>{lngStr('general.actions.confirm')}</Button>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(PlanOfpExerciseEdit))