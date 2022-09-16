import React, { Component } from 'react';
import { Nav, NavItem, NavLink, TabContent, TabPane, Row, Col, InputGroup, InputGroupText, Input } from "reactstrap";
import { LineChartPanel } from "./LineChartPanel";
import { GetAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";
import classnames from 'classnames';

class PlanAnaliticsView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      analitics: [],
      activeTabId: 1,
      loading: true
    };
  }

  componentDidMount() { this.getPlanAnalitics(); }

  getPlanAnalitics = async () => {
    var analiticsData = await GetAsync("/analitics/getPlanAnalitics");

    this.setState({ analitics: analiticsData, loading: false });
  }

  toggleTab = tabId => {
    if (this.state.activeTabId !== tabId) {
      this.setState({ activeTabId: tabId });
    }
  }

  render() {
    if (!this.state.loading) {
      return (
        <>
          <h3>Аналитика</h3>
          <p style={{ marginBottom: '30px' }}>Графики изменения расчетных показателей тренировочных планов во времени</p>

          <p><em>Загрузка...</em></p>);
        </>);
    }

    const data = [
      { name: 'Page A', uv: 4000, pv: 2400, amt: 2400, },
      { name: 'Page B', uv: 3000, pv: 1398, amt: 2210, },
      { name: 'Page C', uv: 2000, pv: 9800, amt: 2290, },
      { name: 'Page D', uv: 2780, pv: 3908, amt: 2000, },
      { name: 'Page E', uv: 1890, pv: 4800, amt: 2181, },
      { name: 'Page F', uv: 2390, pv: 3800, amt: 2500, },
      { name: 'Page G', uv: 3490, pv: 4300, amt: 2100, },
    ];

    return (
      <>
        <h3>Аналитика</h3>
        <p>Графики изменения расчетных показателей тренировочных планов во времени</p>

        <Row style={{ marginBottom: '30px', marginTop: '30px' }}>
          <Col xs={6}>
            <InputGroup>
              <InputGroupText>Построение графиков по тренировочным планам с:</InputGroupText>
              <Input type="date" value={new Date().setFullYear(2022, 8, 1)} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>по:</InputGroupText>
              <Input type="date" value={new Date()} />
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
              Вес
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
            <LineChartPanel data={data} />
          </TabPane>

          <TabPane tabId="2">
            <LineChartPanel data={data} />
          </TabPane>

          <TabPane tabId="3">
            <LineChartPanel data={data} />
          </TabPane>

          <TabPane tabId="4">
            <LineChartPanel data={data} />
          </TabPane>

        </TabContent>
      </>
    );
  }


}

export default WithRouter(PlanAnaliticsView)