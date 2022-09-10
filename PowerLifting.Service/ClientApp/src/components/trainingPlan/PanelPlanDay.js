import React from 'react';

function defaultRowDblClick(settings) { }


export function PanelPlanDay({ planDay, percentages, rowDblClick = defaultRowDblClick }) {
  return (
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
              <td className="text-center" role="button" onDoubleClick={() => rowDblClick(item)}>
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
          {planDay.liftIntensities.map(intensity =>
            <td className="text-center"> {intensity.value} </td>
          )}
          <td className="text-center"><strong>{planDay.liftCounterSum}</strong></td>
          <td className="text-center"><strong>{planDay.weightLoadSum}</strong></td>
          <td className="text-center"><strong>{planDay.intensitySum}</strong></td>
        </tr>
      </tfoot>
    </table>
  );
}

function ExerciseSettingsPanel(settingsData) {
  var settings = settingsData.settings;
  if (settings.weight === 0 && settings.iterations === 0 && settings.exercisePart1 === 0 && settings.exercisePart2 === 0 && settings.exercisePart3 === 0) {
    return (<> - </>);
  }

  return (
    <>
      {settings.weight} | {settings.iterations} < br /> {settings.exercisePart1} | {settings.exercisePart2} | {settings.exercisePart3}
    </>
  );
}
