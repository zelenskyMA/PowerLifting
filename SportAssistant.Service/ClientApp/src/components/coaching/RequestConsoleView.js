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
    const lngStr = this.props.lngStr;

    if (this.state.myRequests.length === 0) {
      return (<p className="spaceBorder"><em>{lngStr('coaching.request.noRequests')}</em></p>);
    }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('coaching.request.requester'), accessor: 'userName' },
      { Header: lngStr('coaching.request.date'), accessor: 'creationDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <div className="spaceTop">
        <p><strong>{lngStr('coaching.request.requesterList')}</strong></p>
        <TableControl columnsInfo={columns} data={this.state.myRequests} rowClick={this.onRowlClick} />
      </div>
    );
  }
}

export default WithRouter(RequestConsoleView)