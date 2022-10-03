import React, { Component } from 'react';
import { Button, Row, Col } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { ErrorPanel, InputText, InputTextArea, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';


class GroupConsolePanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      myGroups: [],
      newGroup: { name: '', description: '' },
      error: ''
    };
  }

  componentDidMount() { this.getGroupsData(); }

  getGroupsData = async () => {
    var myGroupsData = await GetAsync("/trainingGroups/getList");
    this.setState({ myGroups: myGroupsData, newGroup: { name: '', description: '' } });
  }

  newGroupChange = (propName, value) => {
    this.setState({ error: '' });
    this.setState(prevState => ({ newGroup: { ...prevState.newGroup, [propName]: value } }));
  }

  createGroup = async () => {
    try {
      await PostAsync(`/trainingGroups/create`, this.state.newGroup);
      await this.getGroupsData();
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

        <hr style={{paddingTop: "2px" }} />
        <ErrorPanel errorMessage={this.state.error} />
        
        <Row>
          <Col xs={5}>
            <InputText label="Название новой группы" propName="name" onChange={this.newGroupChange} initialValue={this.state.newGroup.name} />
          </Col>        
          <Col xs={4}>
            <InputTextArea label="Описание" propName="description" rows="2" onChange={this.newGroupChange} initialValue={this.state.newGroup.description} />
          </Col>
        </Row>
        <Button color="primary" onClick={() => this.createGroup()}>Создать</Button>
      </div>
    );
  }
}

export default WithRouter(GroupConsolePanel)