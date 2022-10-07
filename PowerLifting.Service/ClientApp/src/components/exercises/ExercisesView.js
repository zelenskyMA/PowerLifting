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
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Название', accessor: 'name' },
      { Header: 'Тип упражнения', accessor: 'exerciseTypeName' },
      { Header: 'Подтип упражнения', accessor: 'exerciseSubTypeName' }
    ];

    var hasData = this.state.exercises && this.state.exercises.length > 0;

    return (
      <div className="spaceTop">
        {!hasData && (<p><em>У вас нет упражнений для редактирования</em></p>)}
        {hasData && (
          <TableControl columnsInfo={columns} data={this.state.exercises} rowClick={this.onRowClick} />
        )}

        <Button className="spaceTop" color="primary" onClick={() => this.onCreate()}>Создать</Button>
      </div>
    );
  }
}

export default WithRouter(ExercisesView)