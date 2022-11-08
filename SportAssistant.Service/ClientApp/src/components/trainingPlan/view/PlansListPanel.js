import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel, TabControl, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal } from "../../../common/Localization";

/*Используется в нескольких компонентах */
class PlansListPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      activePlans: [],
      expiredPlans: [],
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var plans = await GetAsync(`/trainingPlan/getList?userId=${this.props.groupUserId}`);

    this.setState({ activePlans: plans.activePlans, expiredPlans: plans.expiredPlans, loading: false });
  }

  onRowClick = async (row) => { this.props.navigate(`/editPlanDays/${row.values.id}`); }

  confirmAsync = async () => { this.props.navigate("/"); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Начало тренировок', accessor: 'startDate', Cell: t => DateToLocal(t.value) },
      { Header: 'Окончание тренировок', accessor: 'finishDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <>
        <TabControl data={[
          { id: 1, label: 'Действующие', renderContent: () => this.activePlansContent(columns) },
          { id: 2, label: 'История', renderContent: () => this.historyPlansContent(columns) }
        ]}
        />
      </>
    );
  }

  activePlansContent = (columns) => {
    return (
      <>
        <TableControl columnsInfo={columns} data={this.state.activePlans} rowClick={this.onRowClick} pageSize={10} hideFilter={true} />
        <Button style={{ marginTop: '20px' }} color="primary" onClick={() => this.props.navigate(`/createPlan/${this.props.groupUserId}`)}>
          Запланировать тренировки
        </Button>
      </>
    );
  }

  historyPlansContent = (columns) => {
    return (
      <TableControl columnsInfo={columns} data={this.state.expiredPlans} rowClick={this.onRowClick} pageSize={10} hideFilter={true} />
    );
  }
}

export default WithRouter(PlansListPanel);