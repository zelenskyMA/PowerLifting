import React, { Component } from 'react';
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PutAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputTextArea, LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class TemplateOfpExerciseEdit extends Component {
  constructor() {
    super();

    this.state = {
      templateExercise: Object,
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var templateExerciseData = await GetAsync(`/templateExercise/${this.props.params.id}`);

    this.setState({ templateExercise: templateExerciseData, loading: false });
  }

  confirmAsync = async () => {
    try {
      var templateExercise = this.state.templateExercise;

      await PutAsync('/templateExercise', { templateExercise: templateExercise });
      this.props.navigate(`/editTemplateDay/${this.props.params.templateId}/${templateExercise.templateDayId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onExerciseChange = (propName, value) => { this.setState(prevState => ({ error: '', templateExercise: { ...prevState.templateExercise, [propName]: value } })); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4><i>{this.state.templateExercise.exercise.name}</i></h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={5}>
            <InputTextArea onChange={this.onExerciseChange} propName="comments" cols={93}
              label={lngStr('training.leaveComment')} initialValue={this.state.templateExercise.comments} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={5}>
            <div>{lngStr('training.exercise.target') + ':'}</div>
            <InputTextArea onChange={this.onExerciseChange} propName="extPlanData" cols={85}
              label={lngStr('training.externalTraining')} initialValue={this.state.templateExercise.extPlanData} />
          </Col>
        </Row>

        <Button className="spaceTop spaceRight" color="primary" onClick={() => this.confirmAsync()}>{lngStr('general.actions.confirm')}</Button>
      </>
    );
  }
}

export default WithRouter(TemplateOfpExerciseEdit)