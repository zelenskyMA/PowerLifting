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
      await PostAsync(`/trainingGroups/create`, this.state.newGroup);
      await this.getInitData();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onRowClick = row => { this.props.navigate(`/trainingGroup/${row.values.id}`); }

  render() {
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Название', accessor: 'name' },
      { Header: 'Кол-во участников', accessor: 'participantsCount' }
    ];

    var hasData = this.state.myGroups && this.state.myGroups.length > 0;

    return (
      <div className="spaceTop">
        {!hasData && (<p><em>У вас нет групп</em></p>)}
        {hasData && (
          <TableControl columnsInfo={columns} data={this.state.myGroups} rowClick={this.onRowClick} />
        )}

        <hr style={{ paddingTop: "2px" }} />
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={8}>
            <InputText label="Название новой группы" propName="name" onChange={this.newGroupChange} initialValue={this.state.newGroup.name} />
          </Col>
          <Col xs={4}>
            <InputTextArea label="Описание" propName="description" rows="2" cols="45" onChange={this.newGroupChange} initialValue={this.state.newGroup.description} />
          </Col>
        </Row>
        <Button color="primary" onClick={() => this.createGroup()}>Создать группу</Button>
      </div>
    );
  }
}

export default WithRouter(GroupConsoleView)