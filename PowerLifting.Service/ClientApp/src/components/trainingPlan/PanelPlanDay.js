import React from 'react';
import { UncontrolledTooltip } from 'reactstrap';
import { WeightCount } from "../../common/Localization";

function defaultRowDblClick(settings) { }

export function PanelPlanDay({ planDay, percentages = [], achivements = [], rowDblClick = defaultRowDblClick, mode = 'View' }) {
  const idPrefix = 'col';

  return (
    <>
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th style={{ width: "250px" }}>Упражнение</th>
            {percentages.map((item, i) => <th className="text-center">{item.name}</th>)}
            <th style={{ width: "25px" }}>КПШ</th>
            <th style={{ width: "25px" }}>Нагрузка</th>
            <th style={{ width: "25px" }}>Интенсивность</th>
          </tr>
        </thead>
        <tbody>
          {planDay.exercises.map((planExercise, i) =>
            <tr>
              <td>{planExercise.exercise.name}</td>
              {planExercise.settings.map(item =>
                <>
                  <td className="text-center" role="button" id={idPrefix + item.id} onDoubleClick={() => rowDblClick(item)}>
                    <ExerciseSettingsPanel settings={item} />
                  </td>
                </>
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
            {planDay.liftIntensities.map(intensity =>
              <td className="text-center"> {intensity.value} </td>
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

function Tooltips({ planDay, idPrefix, mode, percentages, achivements }) {
  return (<>
    {planDay.exercises.map((planExercise, i) =>
      planExercise.settings.map(item => {

        if (mode === 'View' || item.weight != 0) {
          return (<></>);
        }

        var percentage = percentages.find(t => t.id === item.percentage.id);
        var achivement = achivements.find(t => t.exerciseTypeId === planExercise.exercise.exerciseTypeId);

        return (
          <UncontrolledTooltip placement="top" target={String(idPrefix + item.id)}>
            Вес: {WeightCount(achivement?.result, percentage?.minValue)} - {WeightCount(achivement?.result, percentage?.maxValue)}
          </UncontrolledTooltip>);
      })
    )}
  </>);
}

function ExerciseSettingsPanel({ settings }) {
  if (settings.weight === 0) {
    return (<> - </>);
  }

  return (
    <>
      {settings.weight} | {settings.iterations} < br /> {settings.exercisePart1} | {settings.exercisePart2} | {settings.exercisePart3}
    </>
  );
}
