import React, { Component } from 'react';
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { ErrorPanel, InputText, InputTextArea, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';


class GroupConsoleView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      myGroups: [],
      newGroup: { name: '', description: '' },
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var myGroupsData = await GetAsync("/trainingGroups/getList");
    this.setState({ myGroups: myGroupsData, newGroup: { name: '', description: '' } });
  }

  newGroupChange = (propName, value) => { this.setState(prevState => ({ error: '', newGroup: { ...prevState.newGroup, [propName]: value } })); }

  createGroup = async () => {
    try {
      await PostAsync(`/trainingGroups`, this.state.newGroup);
      await this.getInitData();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onRowClick = row => { this.props.navigate(`/trainingGroup/${row.values.id}`); }

  render() {
    const lngStr = this.props.lngStr;

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('general.common.name'), accessor: 'name' },
      { Header: lngStr('coaching.groups.numberOfUsers'), accessor: 'participantsCount' }
    ];

    var hasData = this.state.myGroups && this.state.myGroups.length > 0;

    return (
      <div className="spaceTop">
        {!hasData && (<p><em>{lngStr('coaching.groups.noGroups')}</em></p>)}
        {hasData && (
          <TableControl columnsInfo={columns} data={this.state.myGroups} rowClick={this.onRowClick} pageSize={10} />
        )}

        <hr style={{ paddingTop: "2px" }} />
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={8}>
            <InputText label={lngStr('coaching.groups.newGroupName')} propName="name" onChange={this.newGroupChange} initialValue={this.state.newGroup.name} />
          </Col>
          <Col xs={4}>
            <InputTextArea label={lngStr('general.common.description')} propName="description" rows="2" cols="45" onChange={this.newGroupChange} initialValue={this.state.newGroup.description} />
          </Col>
        </Row>
        <Button color="primary" onClick={() => this.createGroup()}>{lngStr('coaching.groups.createGroup')}</Button>
      </div>
    );
  }
}

export default WithRouter(GroupConsoleView)