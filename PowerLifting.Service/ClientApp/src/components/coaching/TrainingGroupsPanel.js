import React, { Component } from 'react';
import { Button } from "reactstrap";
import { TableControl } from "../../common/controls/CustomControls";
import { GetAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';


class TrainingGroupsPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      myGroups: []
    };
  }

  componentDidMount() { this.getGroupsData(); }

  getGroupsData = async () => {
    var myGroupsData = await GetAsync("/trainingGroups/get");
    this.setState({ myGroups: myGroupsData });
  }

  addGroup = () => { }

  onRowlClick = row => {
  }

  render() {   
    const columns = [
      { Header: 'Id', accessor: 'userId' },
      { Header: 'Название', accessor: 'name' },
      { Header: 'Кол-во участников', accessor: 'participantCount' }
    ];

    return (
      <div className="spaceTop">
        {this.state.myGroups.length === 0 && (<p><em>У вас нет групп спортсменов</em></p>)}
        {this.state.myGroups.length !== 0 && (
          <>
            <p>Список групп.</p>
            <TableControl columnsInfo={columns} data={this.state.myGroups} rowClick={this.onRowlClick} />
          </>
        )}

        <Button className="spaceTop" color="primary" onClick={() => this.addGroup()}>Создать</Button>
      </div>
    );
  }
}

export default WithRouter(TrainingGroupsPanel)