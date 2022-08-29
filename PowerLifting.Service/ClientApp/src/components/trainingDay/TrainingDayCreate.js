import React, { Component } from 'react';
import { TrainingDayModel } from "../../common/models/WorkDayModel";

export class TrainingDayCreate extends Component {
  static displayName = TrainingDayCreate.name;

  createPlan = e => {
    const year = +e.currentTarget.innerText
    this.props.createTrainingPlan(year) // setYear -> getPhotos
  }

  setUserId() {
    this.setState({
      trainingDay: {
        ...this.state.trainingDay,
        userId: event.target.value
      }
    });
  }

  render() {
    return (
      <div>
        <h1>Создание плана тренировок</h1>
        <p>Тест</p>

        <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

        <input type='text' onChange={this.setUserId()} value={this.state.trainingDay.userId} />

        <button className="btn btn-primary" onClick={this.createPlan}>Создать</button>
      </div>
    );
  }
}

Page.propTypes = {
  createTrainingPlan: PropTypes.func.isRequired,
  isFetching: PropTypes.bool.isRequired,
}
