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

class TemplateExercisesEdit extends Component {
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
    const [exerciseList, templateExercises] = await Promise.all([
      GetAsync("/exerciseInfo/getPlanningList"),
      GetAsync(`/templateExercise/getByDay?dayId=${this.props.params.dayId}`)
    ]);

    var templateExercisesData = templateExercises.map((item, i) => item.exercise);
    this.setState({ exercises: exerciseList, selectedExercises: templateExercisesData });
  }

  confirmExercisesAsync = async () => {
    await PostAsync('/templateExercise/create', { dayId: this.props.params.dayId, exercises: this.state.selectedExercises });
    this.props.navigate(`/editTemplateDay/${this.props.params.templateId}/${this.props.params.dayId}`);
  }

  onRowDblClick = row => {
    const maxExercises = this.props.appSettings.maxExercises;
    const lngStr = this.props.lngStr;

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
          <Button color="primary" outline onClick={() => this.props.navigate(`/editTemplatePlan/${this.props.params.templateId}`)}>{lngStr('button.back')}</Button>
        </Container>
      </>
    );
  }

}

export default WithRouter(connect(mapStateToProps, null)(TemplateExercisesEdit));