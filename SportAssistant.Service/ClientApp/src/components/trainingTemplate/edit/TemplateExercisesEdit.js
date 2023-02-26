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
      templateDay: Object,
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    const [exerciseList, templateExercises, templateDay] = await Promise.all([
      GetAsync("/exerciseInfo/getPlanningList"),
      GetAsync(`/templateExercise/getByDay/${this.props.params.dayId}`),
      GetAsync(`/templateDay/${this.props.params.dayId}`),
    ]);

    var templateExercisesData = templateExercises.map((item, i) => item.exercise);
    this.setState({ exercises: exerciseList, selectedExercises: templateExercisesData, templateDay: templateDay });
  }

  confirmExercisesAsync = async () => {
    await PostAsync('/templateExercise', { dayId: this.props.params.dayId, exercises: this.state.selectedExercises });
    this.props.navigate(`/editTemplateDay/${this.props.params.templateId}/${this.props.params.dayId}`);
  }

  onRowDblClick = row => {
    const maxExercises = this.props.appSettings.maxExercises;
    const lngStr = this.props.lngStr;

    if (this.state.selectedExercises.length >= maxExercises) {
      this.setState({ error: `${lngStr('general.common.max')} ${maxExercises} ${lngStr('training.exercise.perDay')}.` });
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
      { Header: lngStr('general.common.name'), accessor: 'name' },
      { Header: lngStr('training.exercise.subType'), accessor: 'exerciseSubTypeName' },
      { Header: lngStr('training.exercise.type'), accessor: 'exerciseTypeName' },
    ];

    return (
      <>
        <h4>{lngStr('training.exercise.exercises')} {lngStr('general.common.day') + ' ' + this.state.templateDay.dayNumber}</h4>
        <p><strong>{lngStr('training.exercise.list')}</strong> {lngStr('appSetup.control.dblClickSelect')}</p>
        <TableControl columnsInfo={columns} data={this.state.exercises} rowDblClick={this.onRowDblClick} />

        <p><strong>{lngStr('training.exercise.selected')}</strong> {lngStr('appSetup.control.dblClickRemove')}</p>
        <ErrorPanel errorMessage={this.state.error} />

        <Container fluid>
          <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th style={{ width: '25px' }}></th>
                <th>{lngStr('general.common.name')}</th>
                <th>{lngStr('training.exercise.subType')}</th>
                <th>{lngStr('training.exercise.type')}</th>
              </tr>
            </thead>
            <tbody>
              {this.state.selectedExercises.map((row, index) =>
                <tr key={index} role="button">
                  <td>
                    <span onClick={() => this.rowMoveUp(index)} style={{ paddingRight: '7px' }} title={lngStr('general.common.up')} >
                      🔼
                    </span>
                    <span onClick={() => this.rowMoveDown(index)} title={lngStr('general.common.down')}>
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

          <Button color="primary" className="spaceRight" onClick={() => this.confirmExercisesAsync()}>{lngStr('general.actions.confirm')}</Button>
          <Button color="primary" outline onClick={() => this.props.navigate(`/editTemplatePlan/${this.props.params.templateId}`)}>{lngStr('general.actions.back')}</Button>
        </Container>
      </>
    );
  }

}

export default WithRouter(connect(mapStateToProps, null)(TemplateExercisesEdit));