import React, { Component } from 'react';
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { LoadingPanel, TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { MessengersPanel } from './ContactsPanels';

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
      { Header: lngStr('appSetup.user.age'), accessor: 'age' },
      { Header: lngStr('general.common.tel'), accessor: 'contacts.telNumber' },
      { Header: lngStr('appSetup.user.messengers'), accessor: 'telegram', Cell: t => this.messengersCell(t) }
    ];

    return (
      <>
        <h4>{lngStr('coaching.trainers')}</h4>
        <p>{lngStr('coaching.request.requestTrainer')}</p>
        <TableControl columnsInfo={columns} data={this.state.coaches} rowClick={this.onRowDblClick} />
      </>
    );
  }

  messengersCell = (record) => {
    return (
      <MessengersPanel contacts={record.row.original.contacts} />
    )
  }
}

export default WithRouter(CoachSelection)