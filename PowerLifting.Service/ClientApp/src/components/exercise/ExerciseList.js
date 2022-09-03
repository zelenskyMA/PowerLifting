import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { GetAsync, PostAsync } from "../../common/ApiActions"
import { GoToButton } from "../../common/Navigation";
import { TableView } from "../../common/TableView";

export class ExerciseList extends Component {
  static displayName = ExerciseList.name;

  constructor() {
    super();

    this.state = {
      exercises: [],
      selectedExercises: []
    };
  }

  componentDidMount() {
    this.getExercises();
  }

  async getExercises() {
    var data = await GetAsync("exercise/getList");
    this.setState({ exercises: data });
  }

  confirmExercisesAsync = () => { return PostAsync("plannedExercise/create?trainingDayId=1", this.state.selectedExercises); }

  onRowDblClick = row => {
    const maxExercises = 10;
    if (this.state.selectedExercises.length >= maxExercises) {
      alert(`Максимум ${maxExercises} упражнений для одной тренировки.`);
      return;
    }

    var element = row.values;
    this.setState(previousState => ({
      selectedExercises: [...previousState.selectedExercises, element]
    }));
  }

  rowRemove = (index) => {
    var data = this.state.selectedExercises.filter((v, i) => i !== index);
    this.setState({ selectedExercises: data })
  }

  rowMoveUp = (index) => {
    if (index === 0) { return; }

    var data = [...this.state.selectedExercises];
    var temp = data[index - 1];
    data[index - 1] = data[index];
    data[index] = temp;

    this.setState({ selectedExercises: data })
  }

  rowMoveDown = (index) => {
    if (index === this.state.selectedExercises.length - 1) { return; }

    var data = [...this.state.selectedExercises];
    var temp = data[index + 1];
    data[index + 1] = data[index];
    data[index] = temp;

    this.setState({ selectedExercises: data })
  }

  render() {
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Название', accessor: 'name' },
      { Header: 'Тип упражнения', accessor: 'exerciseTypeName' },
      { Header: 'Описание', accessor: 'description' },
    ];

    return (
      <>
        <h1>Упражнения</h1>
        <p><strong>Список упражнений.</strong> Выбрать двойным нажатием.</p>
        <TableView columnsInfo={columns} data={this.state.exercises} rowDblClick={this.onRowDblClick} />

        <p><strong>Выбранные упражнения.</strong> Убрать лишнее двойным нажатием.</p>
        <Container fluid>
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th style={{ width: '25px' }}></th>
                <th>Название</th>
                <th>Тип упражнения</th>
                <th>Описание</th>
              </tr>
            </thead>
            <tbody>
              {this.state.selectedExercises.map((row, index) =>
                <tr key={index} role="button">
                  <td>
                    <span onClick={() => this.rowMoveUp(index)} style={{ paddingRight: '7px' }} title="Вверх" >
                      🔼
                    </span>
                    <span onClick={() => this.rowMoveDown(index)} title="Вниз">
                      🔽
                    </span>
                  </td>
                  <td onDoubleClick={() => this.rowRemove(index)}>{row.name}</td>
                  <td onDoubleClick={() => this.rowRemove(index)}>{row.exerciseTypeName}</td>
                  <td onDoubleClick={() => this.rowRemove(index)}>{row.description}</td>
                </tr>
              )}
            </tbody>
          </table>

          <GoToButton url="/addTrainingPlan" beforeNavigate={this.confirmExercisesAsync} />
        </Container>
      </>
    );
  }
}