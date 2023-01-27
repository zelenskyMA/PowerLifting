import React from 'react';
import { useTranslation } from 'react-i18next';
import { UncontrolledTooltip } from "reactstrap";
import '../../../styling/Common.css';

export function ExerciseSettingsEditPanel({ percentage, settings }) {
  const { t } = useTranslation();
  var settingsList = settings.filter(t => t.percentage.id === percentage.id).sort((a, b) => a.weight - b.weight);

  if (settingsList.length === 0 || settingsList.filter(t => t.weight !== 0).length === 0) {
    return (<div> - </div>);
  }

  var idPrefix = String(`settings_${percentage.id}_${settingsList[0].id}`);
  var imgPrefix = '/img/table/barbell';

  return (
    <>
      <div className="text-center" id={idPrefix}>
        <img src={settingsList[0].completed ? `${imgPrefix}Completed.png` : `${imgPrefix}Planned.png`} width="30" height="35" className="rounded mx-auto d-block" />
      </div>
      <Tooltip settingsList={settingsList} idPrefix={idPrefix} lngStr={t} />
    </>
  );
}

function Tooltip({ settingsList, idPrefix, lngStr }) {
  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      {settingsList.map(settings => {
        return (
          <p key={`tooltip_${idPrefix}${Math.random()}`}>
            {
              `${lngStr('training.weight')}: ${settings.weight} ${lngStr('training.iterations')}: ${settings.iterations} ${lngStr('training.repeates')}: ${settings.exercisePart1} | ${settings.exercisePart2} | ${settings.exercisePart3}`
            }
          </p>
        );
      })}
    </UncontrolledTooltip>
  );
}