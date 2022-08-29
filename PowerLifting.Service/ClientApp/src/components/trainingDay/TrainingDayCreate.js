import React, { Component } from 'react';
import { TrainingDayModel } from "../../common/models/WorkDayModel";

export class TrainingDayCreate extends Component {
  static displayName = TrainingDayCreate.name;

  constructor(props) {
    super(props);
    this.state = { trainingDay: TrainingDayModel };

    this.incrementCounter = this.incrementCounter.bind(this);
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

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}