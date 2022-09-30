import React, { Component } from 'react';
import { TableControl } from "../../common/controls/CustomControls";
import { GetAsync } from "../../common/ApiActions";
import { DateToLocal } from "../../common/Localization";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';


class TrainingRequestsPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      myRequests: []
    };
  }

  componentDidMount() { this.getRequestsData(); }

  getRequestsData = async () => {
    var myRequestsData = await GetAsync("/trainingRequests/getCoachRequests");
    this.setState({ myRequests: myRequestsData });
  }

  onRowlClick = row => {
  }

  render() {
    if (this.state.myRequests.length === 0) {
      return (<p><em>У вас нет заявок, ожидающих принятия</em></p>);
    }

    const columns = [
      { Header: 'Id', accessor: 'userId' },
      { Header: 'Заявитель', accessor: 'userName' },
      { Header: 'Дата заявки', accessor: 'creationDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <div className="spaceTop">
        <p><strong>Список заявителей.</strong> Принять заявку двойным нажатием.</p>
        <TableControl columnsInfo={columns} data={this.state.myRequests} rowClick={this.onRowlClick} />
      </div>
    );
  }
}

export default WithRouter(TrainingRequestsPanel)