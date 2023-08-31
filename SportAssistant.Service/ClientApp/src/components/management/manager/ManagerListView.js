import React, { Component } from 'react';
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel, TableControl, UserSearchControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class ManagerListView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      managers: [],
      checkedUser: Object,

      loading: true,
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var managersData = await GetAsync(`/manager/getList`);

    this.setState({ managers: managersData, loading: false, error: '' });
  }

  onRowClick = row => { this.props.navigate(`/manager/${row.values.id}`); }

  handleSearchResult = (userCard, errorText) => {
    if (errorText) {
      this.setState({ error: errorText });
      return;
    }

    this.setState({ checkedUser: userCard, error: '' });
  }

  assignManager = async () => {
    try {
      await PostAsync(`/manager/changeRole`, { userId: this.state.checkedUser.userId, roleStatus: true });
      this.getInitData();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    const lngStr = this.props.lngStr;
    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('general.common.userName'), accessor: 'name' },
      { Header: lngStr('management.license.coaches'), accessor: data => { return `${data.allowedCoaches} / ${data.distributedCoaches}`; } }
    ];

    var hasData = this.state.managers && this.state.managers.length > 0;

    return (
      <div className="spaceTop">
        {!hasData && (<p><em>{lngStr('management.noManagers')}</em></p>)}
        {hasData && (
          <TableControl columnsInfo={columns} data={this.state.managers} rowClick={this.onRowClick} pageSize={10} />
        )}

        <hr style={{ paddingTop: "2px" }} />
        <ErrorPanel errorMessage={this.state.error} />

        <p className="spaceTop">{lngStr('management.managersAssignment')}{': '}</p>
        <UserSearchControl handleSearchResult={this.handleSearchResult} />
        {this.SelectedUserPanel(lngStr)}

      </div>
    );
  }

  SelectedUserPanel(lngStr) {
    if (!this.state.checkedUser?.userName) { return (<></>) }

    return (
      <Row className="spaceMinTop">
        <Col xs={2}>
          <Button color="primary" onClick={() => this.assignManager()}>{lngStr('management.assign')}</Button>
        </Col>
        <Col>
          <p className="spaceMinTop">
            <span className="spaceRight"> {lngStr('management.org.userExists')}: </span>
            <b> {this.state.checkedUser?.userName} </b>
          </p>
        </Col>
      </Row>
    );
  }
}

export default WithRouter(ManagerListView)