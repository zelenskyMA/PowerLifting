import React, { Component } from 'react';
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
    await PostAsync(`/trainingRequests/create?coachId=${row.values.id}`);
    this.props.navigate("/userCabinet");
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Имя', accessor: 'name' },
      { Header: 'Возраст', accessor: 'age' }
    ];

    return (
      <>
        <h3>Тренеры</h3>
        <p>Подайте заявку тренеру дважды нажав на его строку.</p>
        <TableControl columnsInfo={columns} data={this.state.coaches} rowClick={this.onRowDblClick} />
      </>
    );
  }
}

export default WithRouter(CoachSelection)