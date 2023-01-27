import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Container } from 'reactstrap';
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";

const mapStateToProps = store => {
  return {
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
    const [exerciseList, planExercises] = await Promise.all([
      GetAsync("/exerciseInfo/getPlanningList"),
      GetAsync(`/planExercise/getByDay?dayId=${this.props.params.id}`)
    ]);

    var planExercisesData = planExercises.map((item, i) => item.exercise);
    this.setState({ exercises: exerciseList, selectedExercises: planExercisesData });
  }

  confirmExercisesAsync = async () => {
    await PostAsync('/planExercise/create', { dayId: this.props.params.id, exercises: this.state.selectedExercises });
    this.props.navigate(`/editPlanDay/${this.props.params.planId}/${this.props.params.id}`);
  }

  onRowDblClick = row => {
    const lngStr = this.props.lngStr;
    const maxExercises = this.props.appSettings.maxExercises;

    if (this.state.selectedExercises.length >= maxExercises) {
      this.setState({ error: `${lngStr('common.max')} ${maxExercises} ${lngStr('training.exercisesPerDay')}.` });
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
    const lngStr = this.props.lngStr;

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('common.name'), accessor: 'name' },
      { Header: lngStr('training.exerciseSubType'), accessor: 'exerciseSubTypeName' },
      { Header: lngStr('training.exerciseType'), accessor: 'exerciseTypeName' },
    ];

    return (
      <>
        <h4>{lngStr('training.exercises')}</h4>
        <p><strong>{lngStr('training.exerciseList')}</strong> {lngStr('control.dblClickSelect')}</p>
        <TableControl columnsInfo={columns} data={this.state.exercises} rowDblClick={this.onRowDblClick} />

        <p><strong>{lngStr('training.selectedExercises')}</strong> {lngStr('control.dblClickRemove')}</p>
        <ErrorPanel errorMessage={this.state.error} />

        <Container fluid>
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th style={{ width: '25px' }}></th>
                <th>{lngStr('common.name')}</th>
                <th>{lngStr('training.exerciseSubType')}</th>
                <th>{lngStr('training.exerciseType')}</th>
              </tr>
            </thead>
            <tbody>
              {this.state.selectedExercises.map((row, index) =>
                <tr key={index} role="button">
                  <td>
                    <span onClick={() => this.rowMoveUp(index)} style={{ paddingRight: '7px' }} title={lngStr('common.up')} >
                      🔼
                    </span>
                    <span onClick={() => this.rowMoveDown(index)} title={lngStr('common.down')}>
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

          <Button color="primary" className="spaceRight" onClick={() => this.confirmExercisesAsync()}>{lngStr('button.confirm')}</Button>
          <Button color="primary" outline onClick={() => this.props.navigate(`/editPlanDays/${this.props.params.planId}`)}>{lngStr('button.back')}</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(PlanExercisesEdit));