import React, { Component } from 'react';
import { connect } from "react-redux";
import { Col, Row } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import { InputDate, LineChartControl, TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

const mapStateToProps = store => {
  return {
    groupUserId: store.coach.groupUserId,
  }
}

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
      loading: true
    };
  }

  componentDidMount() { this.getPlanAnalitics(); }

  getPlanAnalitics = async () => {
    var utcStartDate = new Date(this.state.startDate.getTime() - this.state.startDate.getTimezoneOffset() * 60 * 1000);
    var utcFinishDate = new Date(this.state.finishDate.getTime() - this.state.finishDate.getTimezoneOffset() * 60 * 1000);

    var request = `startDate=${utcStartDate.toISOString()}&finishDate=${utcFinishDate.toISOString()}`;
    request = this.props.groupUserId ? request + `&userId=${this.props.groupUserId}` : request;

    var analiticsData = await GetAsync(`/analitics/getPlanAnalitics?${request}`);

    this.setState({ analitics: analiticsData, loading: false });
  }

  onValueChange = async (propName, value) => {
    this.setState({ [propName]: new Date(value) });
    await this.getPlanAnalitics();
  }

  render() {
    if (this.state.loading) { return (<p><em>Загрузка...</em></p>); }

    return (
      <>
        <Row className="spaceBottom">
          <Col xs={4}>
            <p>Построение графиков по тренировочным планам</p>
          </Col>
          <Col xs={3}>
            <InputDate label="с:" propName="startDate" onChange={this.onValueChange} initialValue={this.state.startDate?.toISOString()?.substring(0, 10)} />
          </Col>
          <Col xs={3}>
            <InputDate label="по:" propName="finishDate" onChange={this.onValueChange} initialValue={this.state.finishDate?.toISOString()?.substring(0, 10)} />
          </Col>
        </Row>

        <TabControl data={[
          { id: 1, label: 'КПШ', renderContent: () => this.Panel1Content() },
          { id: 2, label: 'Нагрузка', renderContent: () => this.Panel2Content() },
          { id: 3, label: 'Интенсивность', renderContent: () => this.Panel3Content() },
          { id: 4, label: 'Типы упражнений', renderContent: () => this.Panel4Content() },
        ]} />
      </>
    );
  }

  Panel1Content = () => { return (<LineChartControl data={this.state.analitics.planCounters} displayList={[{ name: 'КПШ', id: 'liftCounterSum' }]} />); }
  Panel2Content = () => { return (<LineChartControl data={this.state.analitics.planCounters} displayList={[{ name: 'Нагрузка', id: 'weightLoadSum' }]} />); }
  Panel3Content = () => { return (<LineChartControl data={this.state.analitics.planCounters} displayList={[{ name: 'Интенсивность', id: 'intensitySum' }]} />); }

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

export default WithRouter(connect(mapStateToProps, null)(PlanAnaliticsPanel))