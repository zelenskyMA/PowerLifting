import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import { TableControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class ExercisesView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      exercises: []
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var exercisesData = await GetAsync("/exerciseInfo/getEditingList");
    this.setState({ exercises: exercisesData });
  }

  onCreate = async () => { this.props.navigate(`/exercises/add`); }
  onRowClick = row => { this.props.navigate(`/exercises/edit/${row.values.id}`); }

  render() {
    const lngStr = this.props.lngStr;

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('general.common.name'), accessor: 'name' },
      { Header: lngStr('training.exercise.type'), accessor: 'exerciseTypeName' },
      { Header: lngStr('training.exercise.subType'), accessor: 'exerciseSubTypeName' },
    ];

    var hasData = this.state.exercises && this.state.exercises.length > 0;

    return (
      <div className="spaceTop">
        {!hasData && (<p><em>{lngStr('training.exercise.nothingToEdit')}</em></p>)}
        {hasData && (
          <TableControl columnsInfo={columns} data={this.state.exercises} rowClick={this.onRowClick} />
        )}

        <Button className="spaceTop" color="primary" onClick={() => this.onCreate()}>{lngStr('general.actions.create')}</Button>
      </div>
    );
  }
}

export default WithRouter(ExercisesView)