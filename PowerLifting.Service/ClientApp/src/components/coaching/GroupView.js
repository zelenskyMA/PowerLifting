import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { ErrorPanel, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { setGroupUserId } from "../../stores/coachingStore/coachActions";
import '../../styling/Common.css';

const mapStateToProps = store => {
  return {
    groupUserId: store.groupUserId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    setGroupUserId: (userId) => setGroupUserId(userId, dispatch)
  }
}

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

  onRowClick = async (row) => {
    await this.props.setGroupUserId(row.values.id);
    this.props.navigate(`/groupUser/${row.values.id}`);
  }

  deleteGroup = async () => {
    try {
      await PostAsync(`/trainingGroups/delete?id=${this.props.params.groupId}`);
      this.props.navigate(`/coachConsole`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }


  render() {
    if (this.state.loading) { return (<p><em>Загрузка...</em></p>); }

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
            <Button className="spaceTop" color="primary" onClick={() => this.deleteGroup()}>Удалить</Button>
          </>
        )}
        {hasData && (
          <>
            <p>Список участников</p>
            <TableControl columnsInfo={columns} data={this.state.users} rowClick={this.onRowClick} />
          </>
        )}
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(GroupView))