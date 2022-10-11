import React, { useState } from 'react';
import { Button, Col, Modal, ModalBody, ModalFooter, ModalHeader, Row, UncontrolledTooltip } from 'reactstrap';
import { PostAsync } from "../../../common/ApiActions";
import '../../../styling/Common.css';
import Completed from '../../../styling/icons/barbellCompleted.png';
import Planned from '../../../styling/icons/barbellPlanned.png';

export function PlanDayViewPanel({ planDay }) {
  const [modal, setModal] = useState(false);
  const [modalData, setModalData] = useState(Object);

  var completed = [];
  planDay.exercises.map((planExercise, i) => {
    planDay.percentages.map(percentage => {
      var state = planExercise.settings.filter(t => t.percentage.id === percentage.id).some(r => r.completed);
      completed = [...completed, { id: planExercise.id + "_" + percentage.id, state: state }];
    })
  });
  const [completedExercises, setCompleted] = useState(completed);

  const completeExercise = async (modalData) => {
    await PostAsync("/trainingPlan/completeExercises", modalData.settings.map(a => a.id));

    var current = completedExercises.find(t => t.id === modalData.id);
    current.state = true;
    setCompleted([...completedExercises.filter(t => t.id !== modalData.id), current]);
  }

  const toggle = () => setModal(!modal);

  return (
    <>
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
              <td id={'exercise' + planExercise.id}>{planExercise.exercise.name}</td>
              <ExerciseTooltip planExercise={planExercise} idPrefix={'exercise' + planExercise.id} />

              {planDay.percentages.map(item =>
                <td key={item.id} className="text-center">
                  <ExerciseSettingsViewPanel percentage={item} planExercise={planExercise} setModalData={setModalData} toggle={toggle} completedExercises={completedExercises} />
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

      <Modal isOpen={modal} toggle={toggle}>
        <ModalHeader toggle={toggle}>{modalData.name}</ModalHeader>
        <ModalBody>
          {modalData.settings?.map((item, index) => {
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
          })}
        </ModalBody>
        <ModalFooter>
          <Button color="success" className="spaceRight" onClick={() => { completeExercise(modalData); toggle(); }}>Подтвердить выполнение</Button>
          <Button color="primary" onClick={toggle}>Закрыть</Button>
        </ModalFooter>
      </Modal>

    </>
  );
}


function ExerciseSettingsViewPanel({ percentage, planExercise, completedExercises, setModalData, toggle }) {
  var idPrefix = String("settings_" + percentage.id);

  var settingsList = planExercise.settings.filter(t => t.percentage.id === percentage.id);
  if (settingsList.length === 0 || settingsList.filter(t => t.weight !== 0).length === 0) {
    return (<div> - </div>);
  }

  var modalData = { id: planExercise.id + "_" + percentage.id, settings: settingsList };
  var exerciseState = completedExercises.find(t => t.id === modalData.id).state;

  return (
    <>
      <div role="button" className="text-center" id={idPrefix} onClick={() => { setModalData(modalData); toggle(); }}>
        <img src={exerciseState ? Completed : Planned} width="30" height="35" className="rounded mx-auto d-block" />
      </div>
      <SettingsTooltip settingsList={settingsList} idPrefix={idPrefix} />
    </>
  );
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