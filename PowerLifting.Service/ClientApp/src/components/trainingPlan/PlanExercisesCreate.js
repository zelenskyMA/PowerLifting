import React, { Component } from 'react';
import { Container, Button } from 'reactstrap';
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { TableView } from "../../common/TableView";
import WithRouter from "../../common/extensions/WithRouter";

class PlanExercisesCreate extends Component {
  constructor() {
    super();

    this.state = {
      exercises: [],
      selectedExercises: []
    };
  }

  componentDidMount() { this.getExercises(); }

  async getExercises() {
    const [settingsList, planExercises] = await Promise.all([
      GetAsync("exercise/getList"),
      GetAsync(`trainingPlan/getPlanExercises?dayId=${this.props.params.id}`)
    ]);

    var planExercisesData = planExercises.map((item, i) => item.exercise);
    this.setState({ exercises: settingsList, selectedExercises: planExercisesData });
  }

  confirmExercisesAsync = async () => {
    await PostAsync(`/trainingPlan/createPlanExercises?dayId=${this.props.params.id}`, this.state.selectedExercises);
    this.props.navigate(`/createPlanDay/${this.props.params.id}`);
  }

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
      { Header: 'Категория упражнения', accessor: 'exerciseSubTypeName' },
      { Header: 'Тип упражнения', accessor: 'exerciseTypeName' },
    ];

    return (
      <>
        <h3>Упражнения</h3>
        <p><strong>Список упражнений.</strong> Выбрать двойным нажатием.</p>
        <TableView columnsInfo={columns} data={this.state.exercises} rowDblClick={this.onRowDblClick} />

        <p><strong>Выбранные упражнения.</strong> Убрать лишнее двойным нажатием.</p>
        <Container fluid>
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th style={{ width: '25px' }}></th>
                <th>Название</th>
                <th>Категория упражнения</th>
                <th>Тип упражнения</th>
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
                  <td onDoubleClick={() => this.rowRemove(index)}>{row.exerciseSubTypeName}</td>
                  <td onDoubleClick={() => this.rowRemove(index)}>{row.exerciseTypeName}</td>
                </tr>
              )}
            </tbody>
          </table>

          <Button color="primary" onClick={() => this.confirmExercisesAsync()}>Подтвердить</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(PlanExercisesCreate);