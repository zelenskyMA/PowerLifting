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

class PlanExerciseSettingsEdit extends Component {
  constructor() {
    super();

    this.state = {
      planExercise: Object,
      achivement: Object,
      settingsList: [],
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var data = await GetAsync(`/planExercise/get?id=${this.props.params.id}`);

    var request = `?planExerciseId=${data.id}&exerciseTypeId=${data?.exercise?.exerciseTypeId}`;
    var achivementData = await GetAsync(`/userAchivement/getByExercise${request}`);

    this.setState({ planExercise: data, settingsList: data.settings, achivement: achivementData, loading: false });
  }

  confirmAsync = async () => {
    try {
      var planExercise = this.state.planExercise;
      planExercise.settings = this.state.settingsList;

      await PostAsync('/planExercise/update', { planExercise: planExercise });
      this.props.navigate(`/editPlanDay/${this.props.params.planId}/${planExercise.planDayId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onExerciseChange = (propName, value) => { this.setState(prevState => ({ error: '', planExercise: { ...prevState.planExercise, [propName]: value } })); }

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
    var template = structuredClone(this.state.planExercise.settingsTemplate);

    var maxLiftItems = this.props.appSettings.maxLiftItems;
    if (this.state.settingsList.length >= maxLiftItems) {
      this.setState({ error: `${lngStr('general.common.ngt')} ${maxLiftItems}.` });
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
        <h4><i>{this.state.planExercise.exercise.name}</i></h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Row style={{ marginTop: '5px', marginBottom: '25px' }}>
          <Col xs={6}>{lngStr('training.exercise.typeRecord')}: {this.state.achivement.result}</Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={5}>
            <InputTextArea onChange={this.onExerciseChange} propName="comments" cols={85}
              label={lngStr('training.leaveComment')} initialValue={this.state.planExercise.comments} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={8}>
            {this.state.settingsList.map((item, i) => this.liftIterationPanel(item, i, lngStr))}
          </Col>
        </Row>

        <Button className="spaceTop spaceRight" color="primary" onClick={() => this.confirmAsync()}>{lngStr('general.actions.confirm')}</Button>
        <Button className="spaceTop" color="primary" outline onClick={() => this.addLiftItem(lngStr)}>{lngStr('training.addLift')}</Button>
      </>
    );
  }

  liftIterationPanel(settings, index, lngStr) {
    var key = settings.id.toString();

    var currentPersent = Math.round((settings.weight * 100) / this.state.achivement.result);

    return (
      <>
        <Label key={'l' + key} for={key} sm={2}><Button close onClick={() => this.onSettingsDelete(index)} />{` ${lngStr('training.entity.lift')} ${(index + 1)}:`}</Label>
        <Row key={'row' + key} id={key}>
          <Col xs={2}>
            <InputNumber label={lngStr('training.entity.weight') + ':'} propName={index + '|weight'} onChange={this.onValueChange} initialValue={settings.weight} />
          </Col>
          <Col xs={1}>
            {currentPersent}%
          </Col>
          <Col xs={4}>
            <MultiNumberInput label={lngStr('training.entity.repeates') + ':'} onChange={this.onValueChange} inputList={[
              { propName: index + '|exercisePart1', initialValue: settings.exercisePart1 },
              { propName: index + '|exercisePart2', initialValue: settings.exercisePart2 },
              { propName: index + '|exercisePart3', initialValue: settings.exercisePart3 }]
            } />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('training.entity.iterations') + ':'} propName={index + '|iterations'} onChange={this.onValueChange} initialValue={settings.iterations} />
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(connect(mapStateToProps, null)(PlanExerciseSettingsEdit))