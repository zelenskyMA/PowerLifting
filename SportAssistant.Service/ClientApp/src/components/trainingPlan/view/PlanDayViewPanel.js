import React, { Component } from 'react';
import { connect } from "react-redux";
import { Col, Row, UncontrolledTooltip } from 'reactstrap';
import { PostAsync } from "../../../common/ApiActions";
import { LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import '../../../styling/Common.css';
import Completed from '../../../styling/icons/barbellCompleted.png';
import Planned from '../../../styling/icons/barbellPlanned.png';

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class PlanDayViewPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      selectedModalData: Object,
      completedExercises: [],
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var completed = [];
    var planDay = this.props.planDay;

    planDay.exercises.map((planExercise, i) => {
      planDay.percentages.map(percentage => {
        var state = planExercise.settings.filter(t => t.percentage.id === percentage.id).some(r => r.completed);
        completed = [...completed, { id: planExercise.id + "_" + percentage.id, state: state }];
      })
    });

    this.setState({ completedExercises: completed });
  }

  onCompleteExercise = async () => {
    await PostAsync("/planExercise/complete", this.state.selectedModalData.settings.map(a => a.id));

    var current = this.state.completedExercises.find(t => t.id === this.state.selectedModalData.id);
    current.state = true;

    var newValue = [...this.state.completedExercises.filter(t => t.id !== this.state.selectedModalData.id), current]
    this.setState({ completedExercises: newValue });
  }

  onShowExerciseModal = (modalData) => {
    this.setState({ selectedModalData: modalData });

    var modalInfo = {
      isVisible: true,
      headerText: modalData.name,
      buttons: [{ name: "Подтвердить выполнение", onClick: this.onCompleteExercise, color: "success" }],
      body: () => {
        return (
          modalData.settings?.map((item, index) => {
            return (
              <Row className="spaceBottom" key={item.id}>
                <Col>
                  <span className="spaceRight"><strong>Поднятие {(index + 1)}:</strong></span>
                  <span className="spaceRight"><i>Вес:</i>{' ' + item.weight}</span>
                  <span className="spaceRight"><i>Подходы:</i>{' ' + item.iterations}</span>
                  <span><i>Повторы:</i>{' '}{item.exercisePart1}{' | '}{item.exercisePart2}{' | '}{item.exercisePart3}</span>
                </Col>
              </Row>
            );
          }))
      }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    var planDay = this.props.planDay;

    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th className="nameColumn" >Упражнение</th>
            {planDay.percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
            <th className="intColumn text-center">КПШ</th>
            <th className="intColumn text-center">Нагрузка</th>
            <th className="intColumn text-center">Интенсивность</th>
          </tr>
        </thead>
        <tbody>
          {planDay.exercises.map((planExercise, i) =>
            <tr key={'planTr' + i}>
              <td id={'exercise' + planExercise.id}>
                {planExercise.exercise.name}
                {planExercise.comments && <strong style={{ color: '#9706EF' }}> * {' '}</strong>}
              </td>
              <ExerciseTooltip planExercise={planExercise} idPrefix={'exercise' + planExercise.id} />

              {planDay.percentages.map(percentage =>
                <td key={percentage.id} className="text-center">
                  {this.exerciseSettingsPanel(percentage, planExercise)}
                </td>
              )}
              <td className="text-center"><strong>{planExercise.liftCounter}</strong></td>
              <td className="text-center"><strong>{planExercise.weightLoad}</strong></td>
              <td className="text-center"><strong>{planExercise.intensity}</strong></td>
            </tr>
          )}
        </tbody>
        <tfoot>
          <tr>
            <td><i>КПШ по зонам интенсивности</i></td>
            {planDay.liftIntensities.map((intensity, i) =>
              <td key={'kph' + i} className="text-center"> {intensity.value} </td>
            )}
            <td className="text-center"><strong>{planDay.liftCounterSum}</strong></td>
            <td className="text-center"><strong>{planDay.weightLoadSum}</strong></td>
            <td className="text-center"><strong>{planDay.intensitySum}</strong></td>
          </tr>
        </tfoot>
      </table>
    );
  }

  exerciseSettingsPanel = (percentage, planExercise) => {
    if (this.state.completedExercises.length === 0) { return (<></>); }

    var itemId = `${planExercise.id}_${percentage.id}`;
    var idPrefix = `settings_${itemId}`;

    var settingsList = planExercise.settings.filter(t => t.percentage.id === percentage.id);
    if (settingsList.length === 0 || settingsList.filter(t => t.weight !== 0).length === 0) {
      return (<div> - </div>);
    }

    var modalData = { id: itemId, name: planExercise.exercise.name, settings: settingsList };
    var exerciseState = this.state.completedExercises.find(t => t.id === modalData.id).state;

    return (
      <>
        <div role="button" className="text-center" id={idPrefix} onClick={() => this.onShowExerciseModal(modalData)}>
          <img src={exerciseState ? Completed : Planned} width="30" height="35" className="rounded mx-auto d-block" />
        </div>
        <SettingsTooltip settingsList={settingsList} idPrefix={idPrefix} />
      </>
    );
  }
}


function SettingsTooltip({ settingsList, idPrefix }) {
  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      {settingsList.map(settings => {
        return (
          <p key={idPrefix + settings.id}>
            {
              `Вес: ${settings.weight} Подходы: ${settings.iterations} Повторы: ${settings.exercisePart1} | ${settings.exercisePart2} | ${settings.exercisePart3}`
            }
          </p>
        );
      })}
    </UncontrolledTooltip>
  );
}

function ExerciseTooltip({ planExercise, idPrefix }) {
  var text = planExercise.comments;
  if (!text) { return (<></>); }

  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      <p>{text}</p>
    </UncontrolledTooltip>
  );
}

export default WithRouter(connect(null, mapDispatchToProps)(PlanDayViewPanel));