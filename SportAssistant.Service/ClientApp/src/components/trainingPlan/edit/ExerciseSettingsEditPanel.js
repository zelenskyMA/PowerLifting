import React from 'react';
import { UncontrolledTooltip } from "reactstrap";
import '../../../styling/Common.css';
import Planned from '../../../styling/icons/barbellPlanned.png';
import Completed from '../../../styling/icons/barbellCompleted.png';

export function ExerciseSettingsEditPanel({ percentage, settings }) {
  var idPrefix = String("settings_" + percentage.id);

  var settingsList = settings.filter(t => t.percentage.id === percentage.id);

  if (settingsList.length === 0 || settingsList.filter(t => t.weight !== 0).length === 0) {
    return (<div> - </div>);
  }

  return (
    <>
      <div className="text-center" id={idPrefix}>
        <img src={settingsList[0].completed ? Completed : Planned} width="30" height="35" className="rounded mx-auto d-block" />
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