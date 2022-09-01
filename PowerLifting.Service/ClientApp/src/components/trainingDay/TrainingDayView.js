import React, { Component } from 'react';

export class TrainingDayView extends Component {
  static displayName = TrainingDayView.name;


  constructor(props) {
    super(props);
    this.state = { trainingDay: null, loading: true };
  }

  componentDidMount() {
    this.loadTrainingDay();
  }

  render() {
    let current = new Date();
    let date = `${current.getDate()}/${current.getMonth() + 1}/${current.getFullYear()}`;

    let ss = new Date().toLocaleTimeString();

    let contents = this.state.loading
      ? <p><em>Загрузка...</em></p>
      : TrainingDayView.renderTrainingTable(this.state.trainingDay);

    return (
      <>
        <h1 id="tabelLabel">Тренировочный день</h1>
        <p>Ваши тренировки на {date}. Тренировочная неделя: 11</p>
        {contents}
      </>
    );
  }

  static renderTrainingTable(trainingDay) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Упражнение</th>
            <th>меньше 50%</th>
            <th>50-60%</th>
            <th>60-70%</th>
            <th>70-80%</th>
            <th>80-90%</th>
            <th>90-100%</th>
            <th>100-110%</th>
            <th>110-120%</th>
            <th>больше 120%</th>
            <th>КПШ</th>
            <th>Нагрузка</th>
            <th>Интенсивность</th>
          </tr>
        </thead>
        <tbody>
          {[...Array(trainingDay.exerciseCount)].map((x, i) =>          
            <tr onDoubleClick={() => alert('test')}>
              <td>{trainingDay.name}</td>
              {trainingDay.data.map(exercise =>
                <td>{exercise.weight} | {exercise.iterationCount} <br /> {exercise.repeateCount1} | {exercise.repeateCount2} | {exercise.repeateCount3}</td>
              )}
              <td>посчитаем</td>
              <td>когда-нибудь</td>
              <td>потом</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  async loadTrainingDay() {
    const response = await fetch('weatherforecast/trainingDay');
    const data = await response.json();    
    this.setState({ workDay: data, loading: false });
  }
}
