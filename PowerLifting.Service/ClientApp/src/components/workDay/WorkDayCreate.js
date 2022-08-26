import React, { Component } from 'react';
import { WorkDayModel } from "../../common/models/WorkDayModel";

export class WorkDayCreate extends Component {
  static displayName = WorkDayCreate.name;

  constructor(props) {
    super(props);
    this.state = { workDay: WorkDayModel };

    this.incrementCounter = this.incrementCounter.bind(this);
  }

  setUserId() {
    this.setState({
      workDay: {
        ...this.state.workDay,
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

        <input type='text' onChange={this.setUserId()} value={this.state.workDay.userId} />

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
      </div>
    );
  }
}