import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { TabControl, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal } from "../../../common/Localization";
import { setTrainingPlan } from "../../../stores/trainingPlanStore/planActions";


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
      expiredPlans: []
    };
  }

  componentDidMount() { this.getPlans(); }

  getPlans = async () => {
    var plans = await GetAsync("/trainingPlan/getList");
    this.setState({ activePlans: plans.activePlans, expiredPlans: plans.expiredPlans });
  }

  onRowDblClick = async (row) => {
    var element = row.values;
    await this.props.setTrainingPlan(element.id);
    this.props.navigate("/createPlanDays");
  }

  confirmAsync = async () => { this.props.navigate("/"); }

  render() {
    return (
      <>
        <h3 style={{ marginBottom: '30px' }}>Тренировочные планы</h3>
        <TabControl data={[
          { id: 1, label: 'Действующие', renderContent: () => this.activePlansContent() },
          { id: 2, label: 'История', renderContent: () => this.historyPlansContent() }
        ]}
        />
      </>
    );
  }


  activePlansContent = () => {
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Начало тренировок', accessor: 'startDate', Cell: t => DateToLocal(t.value) },
      { Header: 'Окончание тренировок', accessor: 'finishDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <>
        <TableControl columnsInfo={columns} data={this.state.activePlans} rowDblClick={this.onRowDblClick} pageSize={10} hideFilter={true} />
        <Button style={{ marginTop: '20px' }} color="primary" onClick={() => this.props.navigate("/createPlan")}>Запланировать тренировки</Button>
      </>
    );
  }

  historyPlansContent = () => {
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Начало тренировок', accessor: 'startDate', Cell: t => DateToLocal(t.value) },
      { Header: 'Окончание тренировок', accessor: 'finishDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <TableControl columnsInfo={columns} data={this.state.expiredPlans} rowDblClick={this.onRowDblClick} pageSize={10} hideFilter={true} />
    );
  }
}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(PlansList))