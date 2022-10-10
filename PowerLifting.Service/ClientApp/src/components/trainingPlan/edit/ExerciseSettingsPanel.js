import React from 'react';
import { UncontrolledTooltip } from "reactstrap";
import '../../../styling/Common.css';
import Barbell from '../../../styling/icons/barbell.png';

export function ExerciseSettingsPanel({ percentage, settings }) {
  var idPrefix = String("settings_" + percentage.id);

  var settingsList = settings.filter(t => t.percentage.id === percentage.id);

  if (settingsList.length === 0 || settingsList.filter(t => t.weight !== 0).length === 0) {
    return (<div> - </div>);
  }

  return (
    <>
      <div className="text-center" id={idPrefix}>
        <img src={Barbell} width="30" height="35" className="rounded mx-auto d-block" />
      </div>
      <Tooltip settingsList={settingsList} idPrefix={idPrefix} />
    </>
  );
}


function Tooltip({ settingsList, idPrefix }) {
  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      {settingsList.map(settings => {
        return (
          <p>
            {
              `Вес: ${settings.weight} Подходы: ${settings.iterations} Повторы: ${settings.exercisePart1} | ${settings.exercisePart2} | ${settings.exercisePart3}`
            }
          </p>
        );
      })}
    </UncontrolledTooltip>
  );
}

//{settings.weight} | {settings.iterations} < br /> {settings.exercisePart1} | {settings.exercisePart2} | {settings.exercisePart3}



/*
export function PlanDayEditPanel({ planDay, percentages = [], achivements = [], mode = 'View' }) {
  const idPrefix = 'col';

  return (
    <>
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th className="nameColumn" >Упражнение</th>
            {percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
            <th className="intColumn text-center">КПШ</th>
            <th className="intColumn text-center">Нагрузка</th>
            <th className="intColumn text-center">Интенсивность</th>
          </tr>
        </thead>
        <tbody>
          {planDay.exercises.map((planExercise, i) =>
            <tr key={'planTr' + i}>
              <td role="button" title="Запланировать" onClick={() => createSettings(planDay, useNavigate)}>{planExercise.exercise.name}</td>

              {planExercise.settings.map(item =>
                <td key={item.id} className="text-center" role="button" id={idPrefix + item.id}
                  onClick={() => openSettings(planDay, item)}>
                  <ExerciseSettingsPanel settings={item} />
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

      <Tooltips planDay={planDay} idPrefix={idPrefix} mode={mode} percentages={percentages} achivements={achivements} />
    </>
  );
}

export function Tooltips({ planDay, idPrefix, mode, percentages, achivements }) {
  return (<>
    {planDay.exercises.map((planExercise, i) =>
      planExercise.settings.map(item => {

        if (mode === 'View' || item.weight != 0) {
          return (<></>);
        }

        var percentage = percentages.find(t => t.id === item.percentage.id);
        var achivement = achivements.find(t => t.exerciseTypeId === planExercise.exercise.exerciseTypeId);

        var text = achivement == null ?
          "Укажите рекоды в профиле" :
          `Вес: ${WeightCount(achivement?.result, percentage?.minValue)} - ${WeightCount(achivement?.result, percentage?.maxValue)}`;

        return (
          <UncontrolledTooltip key={'tooltip' + item.id} placement="top" target={String(idPrefix + item.id)}>
            {text}
          </UncontrolledTooltip>);
      })
    )}
  </>);
}
*/