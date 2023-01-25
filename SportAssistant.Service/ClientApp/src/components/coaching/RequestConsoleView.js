import React, { Component } from 'react';
import { GetAsync } from "../../common/ApiActions";
import { TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { DateToLocal } from "../../common/LocalActions";
import '../../styling/Common.css';

class RequestConsoleView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      myRequests: []
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var myRequestsData = await GetAsync("/trainingRequests/getCoachRequests");
    this.setState({ myRequests: myRequestsData });
  }

  onRowlClick = row => { this.props.navigate(`/acceptRequest/${row.values.id}`); }

  render() {
    if (this.state.myRequests.length === 0) {
      return (<p className="spaceBorder"><em>У вас нет заявок, ожидающих принятия</em></p>);
    }

    const columns = [
      { Header: 'Id', accessor: 'id' },
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

export default WithRouter(RequestConsoleView)