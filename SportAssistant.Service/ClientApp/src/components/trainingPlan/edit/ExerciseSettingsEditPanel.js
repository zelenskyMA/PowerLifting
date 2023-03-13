import React from 'react';
import { useTranslation } from 'react-i18next';
import { UncontrolledTooltip } from "reactstrap";
import '../../../styling/Common.css';

export function ExerciseSettingsEditPanel({ percentage, planExercise }) {
  const { t } = useTranslation();
  var settingsList = planExercise.settings.filter(t => t.percentage.id === percentage.id).sort((a, b) => a.weight - b.weight);

  if (settingsList.length === 0 || settingsList.filter(t => t.weight !== 0).length === 0) {
    return (<div> - </div>);
  }
    
  var imgPrefix = '';
  var exerciseTypeId = parseInt(planExercise.exercise.exerciseTypeId, 10);
  switch (exerciseTypeId) {
    case 3: imgPrefix = '/img/table/ofp'; break;
    default: imgPrefix = '/img/table/barbell'; break;
  }

  var idPrefix = String(`settings_${percentage.id}_${settingsList[0].id}`);
 
  return (
    <>
      <div className="text-center" id={idPrefix}>
        <img src={settingsList[0].completed ? `${imgPrefix}Completed.png` : `${imgPrefix}Planned.png`} width="35" height="35" className="rounded mx-auto d-block" />
      </div>
      <RenderTooltip planExercise={planExercise} settingsList={settingsList} idPrefix={idPrefix} lngStr={t} />
    </>
  );
}

function RenderTooltip({ planExercise, settingsList, idPrefix, lngStr }) {
  var exerciseTypeId = parseInt(planExercise.exercise.exerciseTypeId, 10);

  switch (exerciseTypeId) {
    case 3: return(<OfpTooltip planExercise={planExercise} settingsList={settingsList} idPrefix={idPrefix} />);
    default: return(<Tooltip settingsList={settingsList} idPrefix={idPrefix} lngStr={lngStr} />);
  }
}

function Tooltip({ settingsList, idPrefix, lngStr }) {
  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      {settingsList.map(settings => {
        return (
          <p key={`tooltip_${idPrefix}${Math.random()}`}>
            {
              `${lngStr('training.entity.weight')}: ${settings.weight} ${lngStr('training.entity.iterations')}: ${settings.iterations} ${lngStr('training.entity.repeates')}: ${settings.exercisePart1} | ${settings.exercisePart2} | ${settings.exercisePart3}`
            }
          </p>
        );
      })}
    </UncontrolledTooltip>
  );
}

function OfpTooltip({ planExercise, settingsList, idPrefix }) {
  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      {settingsList.map(i => {
        return (
          <p key={`tooltip_${idPrefix}${Math.random()}`}>{`${planExercise.extPlanData}`}</p>
        );
      })}
    </UncontrolledTooltip>
  );
}