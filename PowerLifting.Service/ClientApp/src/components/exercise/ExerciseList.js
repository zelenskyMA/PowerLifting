import React, { Component } from 'react';
import { GetAsync } from "../../common/ApiActions";
import { TableView } from "../../common/TableView";

export class ExerciseList extends Component {
  static displayName = ExerciseList.name;

  constructor() {
    super();

    this.state = {
      exercises: []
    };
  }

  componentDidMount() {
    this.getExercises();
  }

  async getExercises() {
    var data = await GetAsync("exercise/getList");
    this.setState({ exercises: data });
  }

  render() {
    const columns = [
      { Header: '', accessor: 'id' },
      { Header: 'Название', accessor: 'name' },
      { Header: 'Тип упражнения', accessor: 'exerciseTypeName' },
      { Header: 'Описание', accessor: 'description' },
    ];

    return (
      <>
        <h1>Упражнения</h1>
        <p>Назначте упражнения на выбранный день</p>
        <br />
        <TableView columns={columns} data={this.state.exercises} />
      </>
    );
  }
}