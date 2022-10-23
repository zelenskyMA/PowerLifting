import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Container } from 'reactstrap';
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";

const mapStateToProps = store => {
  return {
    groupUserId: store.coach.groupUserId,
    appSettings: store.app.settings,
  }
}

class PlanExercisesEdit extends Component {
  constructor() {
    super();

    this.state = {
      exercises: [],
      selectedExercises: [],
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    const [settingsList, planExercises] = await Promise.all([
      GetAsync("/exerciseInfo/getPlanningList"),
      GetAsync(`/planExercise/getByDay?dayId=${this.props.params.id}`)
    ]);

    var planExercisesData = planExercises.map((item, i) => item.exercise);
    this.setState({ exercises: settingsList, selectedExercises: planExercisesData });
  }

  confirmExercisesAsync = async () => {
    await PostAsync('/planExercise/create', { dayId: this.props.params.id, exercises: this.state.selectedExercises, userId: this.props.groupUserId });
    this.props.navigate(`/editPlanDay/${this.props.params.planId}/${this.props.params.id}`);
  }

  onRowDblClick = row => {
    const maxExercises = this.props.appSettings.maxExercises;
    if (this.state.selectedExercises.length >= maxExercises) {
      this.setState({ error: `Максимум ${maxExercises} упражнений для одной тренировки.` });
      return;
    }

    var element = row.values;
    this.setState(previousState => ({ error: '', selectedExercises: [...previousState.selectedExercises, element] }));
  }

  rowRemove = (index) => {
    var data = this.state.selectedExercises.filter((v, i) => i !== index);
    this.setState({ error: '', selectedExercises: data })
  }

  rowMoveUp = (index) => {
    if (index === 0) { return; }

    var data = [...this.state.selectedExercises];
    var temp = data[index - 1];
    data[index - 1] = data[index];
    data[index] = temp;

    this.setState({ error: '', selectedExercises: data })
  }

  rowMoveDown = (index) => {
    if (index === this.state.selectedExercises.length - 1) { return; }

    var data = [...this.state.selectedExercises];
    var temp = data[index + 1];
    data[index + 1] = data[index];
    data[index] = temp;

    this.setState({ error: '', selectedExercises: data })
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
        <h4>Упражнения</h4>
        <p><strong>Список упражнений.</strong> Выбрать двойным нажатием.</p>
        <TableControl columnsInfo={columns} data={this.state.exercises} rowDblClick={this.onRowDblClick} />

        <p><strong>Выбранные упражнения.</strong> Убрать лишнее двойным нажатием.</p>
        <ErrorPanel errorMessage={this.state.error} />

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

          <Button color="primary" className="spaceRight" onClick={() => this.confirmExercisesAsync()}>Подтвердить</Button>
          <Button color="primary" outline onClick={() => this.props.navigate(`/editPlanDays/${this.props.params.planId}`)}>Назад</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(PlanExercisesEdit));