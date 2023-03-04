import React from 'react';
import { useTranslation } from 'react-i18next';
import { UncontrolledTooltip } from "reactstrap";
import '../../../styling/Common.css';

export function TemplateSettingsEditPanel({ percentage, settings }) {
  var settingsList = settings.filter(t => t.percentage.id === percentage.id).sort((a, b) => a.weightPercentage - b.weightPercentage);

  if (settingsList.length === 0 || settingsList.filter(t => t.weightPercentage !== 0).length === 0) {
    return (<div> - </div>);
  }

  var idPrefix = String(`settings_${percentage.id}_${settingsList[0].id}`);
  var imgPrefix = '/img/table/barbell';

  return (
    <>
      <div className="text-center" id={idPrefix}>
        <img src={settingsList[0].completed ? `${imgPrefix}Completed.png` : `${imgPrefix}Planned.png`} width="30" height="35" className="rounded mx-auto d-block" />
      </div>
      <Tooltip settingsList={settingsList} idPrefix={idPrefix} />
    </>
  );
}

function Tooltip({ settingsList, idPrefix }) {
  const { t } = useTranslation();

  return (
    <UncontrolledTooltip placement="top" target={idPrefix}>
      {settingsList.map(settings => {
        return (
          <p key={`tooltip_${idPrefix}${Math.random()}`}>
            {
              `${t('training.entity.weight') + ' (%)'}: ${settings.weightPercentage} ${t('training.entity.iterations')}: ${settings.iterations} ${t('training.entity.repeates')}: ${settings.exercisePart1} | ${settings.exercisePart2} | ${settings.exercisePart3}`
            }
          </p>
        );
      })}
    </UncontrolledTooltip>
  );
}