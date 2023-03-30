import React, { Component } from 'react';
import { Col, Row } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import { DateToUtc } from "../../common/LocalActions";
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
      var request = `startDate=${DateToUtc(this.state.startDate).toISOString()}&finishDate=${DateToUtc(this.state.finishDate).toISOString()}`;
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
          { id: 1, label: lngStr('training.entity.liftCounter'), renderContent: () => this.Panel1Content() },
          { id: 2, label: lngStr('training.entity.weightLoad'), renderContent: () => this.Panel2Content() },
          { id: 3, label: lngStr('training.entity.intensity'), renderContent: () => this.Panel3Content() },
          { id: 4, label: lngStr('training.exercise.subTypes'), renderContent: () => this.Panel4Content() },
        ]} />
      </>
    );
  }

  Panel1Content = () => { return (<LineChartControl chartXDots={this.state.analitics.chartDotsList} linesDataList={this.state.analitics.liftCountersByCategory} />); }
  Panel2Content = () => { return (<LineChartControl chartXDots={this.state.analitics.chartDotsList} linesDataList={this.state.analitics.weightLoadsByCategory} />); }
  Panel3Content = () => { return (<LineChartControl chartXDots={this.state.analitics.chartDotsList} linesDataList={this.state.analitics.intensitiesByCategory} />); }
  Panel4Content = () => { return (<LineChartControl chartXDots={this.state.analitics.chartDotsList} linesDataList={this.state.analitics.categoryCounters} />); }

}

export default WithRouter(PlanAnaliticsPanel);