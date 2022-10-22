import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { ErrorPanel, LoadingPanel, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class GroupView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      group: Object,
      users: [],
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getGroupData(); }

  getGroupData = async () => {
    var groupData = await GetAsync(`/trainingGroups/get?id=${this.props.params.groupId}`);
    this.setState({ group: groupData.group, users: groupData.users, error: '', loading: false });
  }

  onRowClick = async (row) => { this.props.navigate(`/groupUser/${row.values.id}`); }

  deleteGroup = async () => {
    try {
      await PostAsync("/trainingGroups/delete", { id: this.props.params.groupId });
      this.props.navigate(`/coachConsole`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }


  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Имя', accessor: 'fullName' },
      { Header: 'Запланировано недель', accessor: 'activePlansCount' },
    ];

    var hasData = this.state.users && this.state.users.length > 0;

    return (
      <>
        <h5>{this.state.group.name}</h5>
        <p>{this.state.group.description}</p>
        <ErrorPanel errorMessage={this.state.error} />

        {!hasData && (
          <>
            <p><em>В группе нет спортсменов</em></p>
            <Button color="primary" className="spaceTop spaceRight" onClick={() => this.deleteGroup()}>Удалить группу</Button>
          </>
        )}
        {hasData && (
          <>
            <p>Список участников</p>
            <TableControl columnsInfo={columns} data={this.state.users} rowClick={this.onRowClick} />
          </>
        )}

        <Button color="primary" className="spaceTop" outline onClick={() => this.props.navigate('/coachConsole')}>Назад</Button>
      </>
    );
  }
}

export default WithRouter(GroupView);