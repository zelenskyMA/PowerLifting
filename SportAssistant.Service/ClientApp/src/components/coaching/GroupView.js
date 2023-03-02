import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button } from "reactstrap";
import { GetAsync, DeleteAsync } from "../../common/ApiActions";
import { ErrorPanel, LoadingPanel, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

const mapStateToProps = store => {
  return {
    userInfo: store.app.myInfo,
  }
}

class GroupView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      group: Object,
      users: [],
      backRedirectUrl: this.props.userInfo?.coachOnly ? `/groupConsole` : `/coachConsole`,
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getGroupData(); }

  getGroupData = async () => {
    var groupData = await GetAsync(`/trainingGroups/${this.props.params.groupId}`);
    this.setState({ group: groupData.group, users: groupData.users, error: '', loading: false });
  }

  onRowClick = async (row) => { this.props.navigate(`/groupUser/${row.values.id}`); }

  deleteGroup = async () => {
    try {
      await DeleteAsync(`/trainingGroups/${this.props.params.groupId}`);
      this.props.navigate(this.state.backRedirectUrl);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('appSetup.user.name'), accessor: 'fullName' },
      { Header: lngStr('coaching.weeksPlanned'), accessor: 'activePlansCount' },
    ];

    var hasData = this.state.users && this.state.users.length > 0;

    return (
      <>
        <h5>{this.state.group.name}</h5>
        <p>{this.state.group.description}</p>
        <ErrorPanel errorMessage={this.state.error} />

        {!hasData && (
          <>
            <p><em>{lngStr('coaching.groups.noSportsmenInGroup')}</em></p>
            <Button color="primary" className="spaceTop spaceRight" onClick={() => this.deleteGroup()}>{lngStr('coaching.groups.deleteGroup')}</Button>
          </>
        )}
        {hasData && <TableControl columnsInfo={columns} data={this.state.users} rowClick={this.onRowClick} />}

        <Button color="primary" className="spaceRight spaceTop" disabled={!hasData} onClick={() => this.props.navigate(`/assignTemplateSet/${this.props.params.groupId}`)}>{lngStr('training.planTraining')}</Button>
        <Button color="primary" className="spaceTop" outline onClick={() => this.props.navigate(this.state.backRedirectUrl)}>{lngStr('general.actions.back')}</Button>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(GroupView))