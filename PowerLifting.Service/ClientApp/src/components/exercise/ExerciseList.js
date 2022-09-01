import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { GetAsync } from "../../common/ApiActions";
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

  onRowDblClick = row => {
    var element = row.values;
    this.setState(previousState => ({
      selectedExercises: [...previousState.selectedExercises, element]
    }));
  }

  removeRow = (index) => {
    var data = this.state.selectedExercises;
    debugger;
    var filtered = data.filter((v, i) => i !== index);
          
    this.setState({ selectedExercises: filtered })
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
                <th>Название</th>
                <th>Тип упражнения</th>
                <th>Описание</th>
              </tr>
            </thead>
            <tbody>
              {this.state.selectedExercises.map((row, index) =>
                <tr role="button" onDoubleClick={() => this.removeRow(index)}>
                  <td>{row.name}</td>
                  <td>{row.exerciseTypeName}</td>
                  <td>{row.description}</td>
                </tr>
              )}
            </tbody>
          </table>
        </Container>
      </>
    );
  }
}