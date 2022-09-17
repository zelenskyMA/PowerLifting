import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Nav, NavItem, NavLink, TabContent, TabPane } from "reactstrap";
import { DateToLocal } from "../../common/Localization";
import { TableControl } from "../../common/controls/CustomControls";
import { GetAsync } from "../../common/ApiActions";
import { setTrainingPlan } from "../../stores/trainingPlanStore/planActions";
import WithRouter from "../../common/extensions/WithRouter";
import classnames from 'classnames';

const mapStateToProps = store => {
  return {
    planId: store.planId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    setTrainingPlan: (planId) => setTrainingPlan(planId, dispatch)
  }
}

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

  onRowDblClick = async (row) => {
    var element = row.values;
    await this.props.setTrainingPlan(element.id);
    this.props.navigate("/createPlanDays");
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
            <TableControl columnsInfo={columns} data={this.state.activePlans} rowDblClick={this.onRowDblClick} pageSize={10} hideFilter={true} />

            <Button style={{ marginTop: '20px' }} color="primary" onClick={() => this.props.navigate("/createPlan")}>Запланировать тренировки</Button>
          </TabPane>

          <TabPane tabId="2">
            <TableControl columnsInfo={columns} data={this.state.expiredPlans} rowDblClick={this.onRowDblClick} pageSize={10} hideFilter={true} />
          </TabPane>

        </TabContent>
      </>
    );
  }


}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(PlansList))