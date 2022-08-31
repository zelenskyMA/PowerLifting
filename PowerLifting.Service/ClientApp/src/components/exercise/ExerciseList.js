import React, { Component } from 'react';
import { GetAsync } from "../../common/ApiActions"

export class ExerciseList extends Component {
  static displayName = ExerciseList.name;

  static columns = [
    { dataField: "name", text: "Название", sort: true },
    { dataField: "exerciseTypeName", text: "Тип упражнения", sort: true },
    { dataField: "description", text: "Описание", sort: true }
  ];

  static defaultSort = [
    {
      dataField: "name",
      order: "desc"
    }
  ];

  constructor() {
    super();

    this.state = {
      exercises: null
    };
  }

  componentDidMount() {
    this.getExercises();
  }

  async getExercises() {    
    var data = await GetAsync("exercise/getList");
    debugger;
    this.setState({ exercises: data });
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