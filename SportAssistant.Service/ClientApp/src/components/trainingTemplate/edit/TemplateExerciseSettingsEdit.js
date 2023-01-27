import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Label, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputNumber, InputTextArea, LoadingPanel, MultiNumberInput } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

const mapStateToProps = store => {
  return {
    appSettings: store.app.settings,
  }
}

class TemplateExerciseSettingsEdit extends Component {
  constructor() {
    super();

    this.state = {
      templateExercise: Object,
      settingsList: [],
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var templateExerciseData = await GetAsync(`/templateExercise/get?id=${this.props.params.id}`);

    this.setState({ templateExercise: templateExerciseData, settingsList: templateExerciseData.settings, loading: false });
  }

  confirmAsync = async () => {
    try {
      var templateExercise = this.state.templateExercise;
      templateExercise.settings = this.state.settingsList;

      await PostAsync('/templateExercise/update', { templateExercise: templateExercise });
      this.props.navigate(`/editTemplateDay/${this.props.params.templateId}/${templateExercise.templateDayId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onExerciseChange = (propName, value) => { this.setState(prevState => ({ error: '', templateExercise: { ...prevState.templateExercise, [propName]: value } })); }

  onValueChange = (propName, value) => {
    var propElement = propName.split('|');
    var index = parseInt(propElement[0], 10);

    var settings = this.state.settingsList[index];
    settings[propElement[1]] = value;

    var settingsList = this.state.settingsList.filter((v, i) => i !== index);
    var resultList = [];
    if (index === 0) {
      resultList = [settings, ...settingsList];
    }
    else {
      resultList = [...settingsList.slice(0, index), settings, ...settingsList.slice(index)];
    }

    this.setState({ error: '', settingsList: resultList });
  }

  addLiftItem = (lngStr) => {
    var template = structuredClone(this.state.templateExercise.settingsTemplate);

    var maxLiftItems = this.props.appSettings.maxLiftItems;
    if (this.state.settingsList.length >= maxLiftItems) {
      this.setState({ error: `${lngStr('common.ngt')} ${maxLiftItems}.` });
      return;
    }

    this.setState({ error: '', settingsList: [...this.state.settingsList, template] });
  }

  onSettingsDelete = (index) => {
    var resultList = this.state.settingsList.filter((v, i) => i !== index);
    this.setState({ error: '', settingsList: resultList });
  }


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
          <Col xs={8}>
            {this.state.settingsList.map((item, i) => this.liftIterationPanel(item, i, lngStr))}
          </Col>
        </Row>

        <Button className="spaceTop spaceRight" color="primary" onClick={() => this.confirmAsync()}>{lngStr('button.confirm')}</Button>
        <Button className="spaceTop" color="primary" outline onClick={() => this.addLiftItem(lngStr)}>{lngStr('training.addLift')}</Button>
      </>
    );
  }

  liftIterationPanel(settings, index, lngStr) {
    var key = settings.id.toString();

    return (
      <>
        <Label key={'l' + key} for={key} sm={2}><Button close onClick={() => this.onSettingsDelete(index)} />{` ${lngStr('training.lift')} ${(index + 1)}:`}</Label>
        <Row key={'row' + key} id={key}>
          <Col xs={3}>
            <InputNumber label={lngStr('common.percent') + ':'} propName={index + '|weightPercentage'} onChange={this.onValueChange} initialValue={settings.weightPercentage} />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('training.iterations') + ':'} propName={index + '|iterations'} onChange={this.onValueChange} initialValue={settings.iterations} />
          </Col>
          <Col xs={4}>
            <MultiNumberInput label={lngStr('training.repeates') + ':'} onChange={this.onValueChange} inputList={[
              { propName: index + '|exercisePart1', initialValue: settings.exercisePart1 },
              { propName: index + '|exercisePart2', initialValue: settings.exercisePart2 },
              { propName: index + '|exercisePart3', initialValue: settings.exercisePart3 }]
            } />
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(connect(mapStateToProps, null)(TemplateExerciseSettingsEdit))