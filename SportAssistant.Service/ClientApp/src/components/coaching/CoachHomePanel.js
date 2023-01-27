/// <reference path="../trainingplan/view/plandayviewpanel.js" />
import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import PlanDayViewPanel from "../trainingPlan/view/PlanDayViewPanel";
import { LoadingPanel, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class CoachHomePanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userGroups: [],
      selectedUser: Object,
      showDetails: false,
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var data = await GetAsync("/trainingGroups/getWorkoutList");
    this.setState({ userGroups: data, loading: false });
  }

  onRowClick = async (row) => {
    var item = this.state.userGroups.find(t => t.id == row.values.id);
    this.setState({ selectedUser: item, showDetails: true });
  }

  onGoToUser = () => { this.props.navigate(`/groupUser/${this.state.selectedUser.id}`); }

  render() {
    const lngStr = this.props.lngStr;

    if (this.state.loading) { return (<LoadingPanel />); }
    if (this.state.userGroups.length === 0) { return (<p><em>{lngStr('coaching.nooneIsTraining')}</em></p>); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('coaching.sportsman'), accessor: 'name' },
      { Header: lngStr('groups.group'), accessor: 'groupName' },
    ];
    if (!this.state.showDetails) {
      return (
        <>
          <p>{lngStr('coaching.whoIsTraining')}:</p>
          <TableControl columnsInfo={columns} data={this.state.userGroups} rowClick={this.onRowClick} pageSize={15} />
        </>
      );
    }

    return (
      <>
        <h4 className="spaceBottom">{lngStr('training.todaysPlanFor') + ' ' + this.state.selectedUser.name}</h4>
        <PlanDayViewPanel planDay={this.state.selectedUser.planDay} />

        <Button className="spaceTop spaceRight" color="primary" onClick={() => this.onGoToUser()}>{lngStr('user.toUserCard')}</Button>
        <Button className="spaceTop" color="primary" onClick={() => this.setState({ showDetails: false })}>{lngStr('button.back')}</Button>
      </>
    );
  }
}

export default WithRouter(CoachHomePanel);