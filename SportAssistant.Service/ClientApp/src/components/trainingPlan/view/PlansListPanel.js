import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel, TabControl, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal } from "../../../common/Localization";

const mapStateToProps = store => {
  return {
    groupUserId: store.coach.groupUserId,
  }
}

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
    var request = this.props.groupUserId ? `?userId=${this.props.groupUserId}` : "";

    var plans = await GetAsync(`/trainingPlan/getList${request}`);
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
        <Button style={{ marginTop: '20px' }} color="primary" onClick={() => this.props.navigate("/createPlan")}>Запланировать тренировки</Button>
      </>
    );
  }

  historyPlansContent = (columns) => {
    return (
      <TableControl columnsInfo={columns} data={this.state.expiredPlans} rowClick={this.onRowClick} pageSize={10} hideFilter={true} />
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(PlansListPanel));