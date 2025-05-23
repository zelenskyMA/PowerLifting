﻿import React, { Component } from 'react';
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { TableControl, LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";

class CoachSelection extends Component {
  constructor(props) {
    super(props);

    this.state = {
      coaches: [],
      loading: true
    };
  }

  componentDidMount() { this.getCoaches(); }

  getCoaches = async () => {
    var coachesData = await GetAsync("/trainingRequests/getCoaches");
    this.setState({ coaches: coachesData, loading: false });
  }

  onRowDblClick = async row => {
    await PostAsync(`/trainingRequests/${row.values.id}`);
    this.props.navigate("/userCabinet");
  }

  render() {
    const lngStr = this.props.lngStr;

    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('appSetup.user.name'), accessor: 'name' },
      { Header: lngStr('appSetup.user.age'), accessor: 'age' }
    ];

    return (
      <>
        <h4>{lngStr('coaching.trainers')}</h4>
        <p>{lngStr('coaching.request.requestTrainer')}</p>
        <TableControl columnsInfo={columns} data={this.state.coaches} rowClick={this.onRowDblClick} />
      </>
    );
  }
}

export default WithRouter(CoachSelection)