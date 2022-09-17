import React, { Component } from 'react';
import { Nav, NavItem, NavLink, TabContent, TabPane, Row, Col, InputGroup, InputGroupText, Input } from "reactstrap";
import { LineChartView } from "./LineChartView";
import { GetAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";
import classnames from 'classnames';

class PlanAnaliticsView extends Component {
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
      activeTabId: 1,
      loading: true
    };
  }

  componentDidMount() { this.getPlanAnalitics(); }

  getPlanAnalitics = async () => {
    var utcStartDate = new Date(this.state.startDate.getTime() - this.state.startDate.getTimezoneOffset() * 60 * 1000);
    var utcFinishDate = new Date(this.state.finishDate.getTime() - this.state.finishDate.getTimezoneOffset() * 60 * 1000);

    var analiticsData = await GetAsync(`/analitics/getPlanAnalitics?startDate=${utcStartDate.toISOString()}&finishDate=${utcFinishDate.toISOString()}`);

    this.setState({ analitics: analiticsData, loading: false });
  }

  setValue = async (event, propName) => {
    var val = event.target.value;
    this.setState({ [propName]: new Date(val) });

    await this.getPlanAnalitics();
  }

  toggleTab = tabId => {
    if (this.state.activeTabId !== tabId) {
      this.setState({ activeTabId: tabId });
    }
  }

  render() {
    if (this.state.loading) {
      return (<p><em>Загрузка...</em></p>);
    }

    var colors = ['#8884d8', '#CF4034', '#239411', '#3C2CDB', '#9712C8', '#03C7EE', '#EF6206'];
    var exerciseTypesData = [];
    for (var i = 0; i < this.state.analitics.typeCounters.length; i++) {
      exerciseTypesData.push({
        id: 'value',
        data: this.state.analitics.typeCounters[i].values,
        name: this.state.analitics.typeCounters[i].name,
        color: colors[i]
      });
    }

    return (
      <>
        <h3>Аналитика</h3>

        <Row style={{ marginBottom: '30px', marginTop: '30px' }}>
          <Col xs={4}>
            <p>Построение графиков по тренировочным планам</p>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>с:</InputGroupText>
              <Input type="date" language="ru-ru" onChange={(e) => this.setValue(e, 'startDate')} defaultValue={this.state.startDate?.toISOString()?.substring(0, 10)} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>по:</InputGroupText>
              <Input type="date" language="ru"  onChange={(e) => this.setValue(e, 'finishDate')} defaultValue={this.state.finishDate?.toISOString()?.substring(0, 10)} />
            </InputGroup>
          </Col>
        </Row>

        <Nav tabs>
          <NavItem role="button" onClick={() => this.toggleTab(1)}>
            <NavLink className={classnames({ active: this.state.activeTabId === 1 })} >
              КПШ
            </NavLink>
          </NavItem>
          <NavItem role="button" onClick={() => this.toggleTab(2)}>
            <NavLink className={classnames({ active: this.state.activeTabId === 2 })} >
              Нагрузка
            </NavLink>
          </NavItem>
          <NavItem role="button" onClick={() => this.toggleTab(3)}>
            <NavLink className={classnames({ active: this.state.activeTabId === 3 })} >
              Интенсивность
            </NavLink>
          </NavItem>
          <NavItem role="button" onClick={() => this.toggleTab(4)}>
            <NavLink className={classnames({ active: this.state.activeTabId === 4 })} >
              Типы упражнений
            </NavLink>
          </NavItem>
        </Nav>
        <TabContent activeTab={this.state.activeTabId.toString()}>

          <TabPane tabId="1">
            <LineChartView data={this.state.analitics.planCounters} displayList={[{ name: 'КПШ', id: 'liftCounterSum', color: '#8884d8' }]} />
          </TabPane>

          <TabPane tabId="2">
            <LineChartView data={this.state.analitics.planCounters} displayList={[{ name: 'Нагрузка', id: 'weightLoadSum', color: '#8884d8' }]} />
          </TabPane>

          <TabPane tabId="3">
            <LineChartView data={this.state.analitics.planCounters} displayList={[{ name: 'Интенсивность', id: 'intensitySum', color: '#8884d8' }]} />
          </TabPane>

          <TabPane tabId="4">
            <LineChartView data={this.state.analitics.fullTypeCounterList} displayList={exerciseTypesData} multidata="true" />
          </TabPane>

        </TabContent>
      </>
    );
  }


}

export default WithRouter(PlanAnaliticsView)