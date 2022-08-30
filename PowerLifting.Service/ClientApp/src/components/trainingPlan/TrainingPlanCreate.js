import React from 'react';
import { connect } from 'react-redux'
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css';
import { createTrainingPlan } from "../../stores/trainingPlanStore/trainingPlanActions";

const mapStateToProps = store => {
  return {
    planId: store.planId,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    createTrainingPlan: (creationDate) => dispatch(createTrainingPlan(creationDate, dispatch))
  }
}

class TrainingPlanCreate extends React.Component {
  constructor(props) {
    super(props);
  }

  state = {
    date: new Date(),
  }

  onDateChange = date => this.setState({ date });
  onPlanCreate = () => { this.props.createTrainingPlan(this.state.date);}

  render() {
    return (
      <div>
        <h1>Создание плана тренировок</h1>
        <br />
        <p>Выберите дату начала тренировок</p>
        <Calendar onChange={this.onDateChange} value={this.state.date} locale="ru" />
        <br />
        <button className="btn btn-primary" onClick={this.onPlanCreate}>Создать</button>
      </div>
    );
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(TrainingPlanCreate)