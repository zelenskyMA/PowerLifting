import React, { Component } from 'react';
import { Col, Row } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import { InputDate, LineChartControl, LoadingPanel, TabControl, ErrorPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class PlanAnaliticsPanel extends Component {
  constructor(props) {
    super(props);

    const startDate = new Date();
    startDate.setMonth(startDate.getMonth() - 2);

    const finishDate = new Date();
    finishDate.setMonth(finishDate.getMonth() + 1);

    this.state = {
      analitics: [],
      startDate: startDate,
      finishDate: finishDate,
      loading: true,
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    try {
      var utcStartDate = new Date(this.state.startDate.getTime() - this.state.startDate.getTimezoneOffset() * 60 * 1000);
      var utcFinishDate = new Date(this.state.finishDate.getTime() - this.state.finishDate.getTimezoneOffset() * 60 * 1000);

      var request = `startDate=${utcStartDate.toISOString()}&finishDate=${utcFinishDate.toISOString()}`;
      var analiticsData = await GetAsync(`/analitics/getPlanAnalitics/${this.props.groupUserId}?${request}`);

      this.setState({ analitics: analiticsData, loading: false });
    }
    catch (error) { this.setState({ error: error.message, loading: false }); }
  }

  onValueChange = async (propName, value) => {
    this.setState({ [propName]: new Date(value) });
    await this.getInitData();
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceBottom">
          <Col xs={4}>
            <p>{lngStr('analitics.buildingGraph')}</p>
          </Col>
          <Col xs={3}>
            <InputDate label={lngStr('general.common.from') + ':'} propName="startDate" onChange={this.onValueChange} initialValue={this.state.startDate?.toISOString()?.substring(0, 10)} />
          </Col>
          <Col xs={3}>
            <InputDate label={lngStr('general.common.to') + ':'} propName="finishDate" onChange={this.onValueChange} initialValue={this.state.finishDate?.toISOString()?.substring(0, 10)} />
          </Col>
        </Row>

        <TabControl data={[
          { id: 1, label: lngStr('training.entity.liftCounter'), renderContent: () => this.Panel1Content(lngStr) },
          { id: 2, label: lngStr('training.entity.weightLoad'), renderContent: () => this.Panel2Content(lngStr) },
          { id: 3, label: lngStr('training.entity.intensity'), renderContent: () => this.Panel3Content(lngStr) },
          { id: 4, label: lngStr('training.exercise.subTypes'), renderContent: () => this.Panel4Content() },
        ]} />
      </>
    );
  }

  Panel1Content = (lngStr) => { return (<LineChartControl data={this.state.analitics.planCounters} displayList={[{ name: lngStr('training.entity.liftCounter'), id: 'liftCounterSum' }]} />); }
  Panel2Content = (lngStr) => { return (<LineChartControl data={this.state.analitics.planCounters} displayList={[{ name: lngStr('training.entity.weightLoad'), id: 'weightLoadSum' }]} />); }
  Panel3Content = (lngStr) => { return (<LineChartControl data={this.state.analitics.planCounters} displayList={[{ name: lngStr('training.entity.intensity'), id: 'intensitySum' }]} />); }

  Panel4Content = () => {
    var exerciseTypesData = [];
    for (var i = 0; i < this.state.analitics.typeCounters.length; i++) {
      exerciseTypesData.push({
        id: 'value',
        data: this.state.analitics.typeCounters[i].values,
        name: this.state.analitics.typeCounters[i].name
      });
    }

    return (<LineChartControl data={this.state.analitics.fullTypeCounterList} displayList={exerciseTypesData} multidata="true" />);
  }

}

export default WithRouter(PlanAnaliticsPanel);