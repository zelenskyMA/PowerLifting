import React, { Component } from 'react';
import { Button, Nav, NavItem, NavLink, TabContent, TabPane } from "reactstrap";
import { DateToLocal } from "../../common/Localization";
import { TableView } from "../../common/TableView";
import { GetAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";
import classnames from 'classnames';

class PlansList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      activePlans: [],
      expiredPlans: [],
      activeTabId: 1,
    };
  }

  componentDidMount() { this.getPlans(); }

  getPlans = async () => {
    var plans = await GetAsync("/trainingPlan/getList");

    this.setState({ activePlans: plans.activePlans, expiredPlans: plans.expiredPlans });
  }

  toggleTab = tabId => {
    if (this.state.activeTabId !== tabId) {
      this.setState({ activeTabId: tabId });
    }
  }

  onRowDblClick = row => {
    var element = row.values;
    alert(`Ид выбранного плана ${element.id}`);
  }

  confirmAsync = async () => { this.props.navigate("/"); }

  render() {
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Начало тренировок', accessor: 'startDate', Cell: t => DateToLocal(t.value) },
      { Header: 'Окончание тренировок', accessor: 'finishDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <>
        <h3 style={{ marginBottom: '30px' }}>Тренировочные планы</h3>

        <Nav tabs>
          <NavItem role="button" onClick={() => this.toggleTab(1)}>
            <NavLink className={classnames({ active: this.state.activeTabId === 1 })} >
              Действующие
            </NavLink>
          </NavItem>
          <NavItem role="button" onClick={() => this.toggleTab(2)}>
            <NavLink className={classnames({ active: this.state.activeTabId === 2 })} >
              История
            </NavLink>
          </NavItem>
        </Nav>
        <TabContent activeTab={this.state.activeTabId.toString()}>

          <TabPane tabId="1">
            <TableView columnsInfo={columns} data={this.state.activePlans} rowDblClick={this.onRowDblClick} pageSize={10} hideFilter={true} />

            <Button style={{ marginTop: '20px' }} color="primary" onClick={() => this.props.navigate("/createPlan")}>Запланировать тренировки</Button>
          </TabPane>

          <TabPane tabId="2">
            <TableView columnsInfo={columns} data={this.state.expiredPlans} rowDblClick={this.onRowDblClick} pageSize={10} hideFilter={true} />
          </TabPane>

        </TabContent>
      </>
    );
  }


}

export default WithRouter(PlansList)