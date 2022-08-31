import React, { Component } from 'react';
import { useTable, useFilters, useGlobalFilter, useAsyncDebounce } from 'react-table'
import 'bootstrap/dist/css/bootstrap.min.css';


export class ExerciseList extends Component {
  constructor() {
    super();

    var columns = [
      { dataField: "name", text: "Название", sort: true },
      { dataField: "exerciseTypeName", text: "Тип упражнения", sort: true },
      { dataField: "description", text: "Описание", sort: true }
    ];

    var defaultSort = [
      {
        dataField: "name",
        order: "desc"
      }
    ];

    this.state = {
      exercises: null,
      columns: columns,
      defaultSort: defaultSort,
    };
  }

  componentDidMount() {
    this.getExercises();
  }

  getExercises = () => {
    fetch("exercise/getList")
      .then(response => response.json())
      .then(data => {
        this.setState({ exercises: data })
      });
  }

  render() {
    return (
      <>
        <h1>Counter</h1>
        <p>This is a simple example of a React component.</p>
      </>
    );
  }
}
