import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel, Tooltip } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal } from "../../../common/LocalActions";
import '../../../styling/Common.css';
import { ExerciseSettingsEditPanel } from "./ExerciseSettingsEditPanel";

class PlanDayEdit extends Component {
  constructor() {
    super();

    this.state = {
      planDay: Object,
      loading: true,
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var data = await GetAsync(`/planDay/${this.props.params.id}`);
    this.setState({ planDay: data, loading: false });
  }

  editSettings = (planExercise) => {
    var exerciseTypeId = parseInt(planExercise.exercise.exerciseTypeId, 10);
    switch (exerciseTypeId) {
      case 3: this.props.navigate(`/editPlanOfpExercise/${this.props.params.planId}/${planExercise.id}`); break;
      default: this.props.navigate(`/editPlanExerciseSettings/${this.props.params.planId}/${planExercise.id}`); break;
    }
  }

  confirmAsync = () => { this.props.navigate(`/editPlanDays/${this.props.params.planId}`); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;
    var dateView = DateToLocal(this.state.planDay.activityDate);
    var ownerName = this.state.planDay?.owner?.name;

    return (
      <>
        <h4 className="spaceBottom">{lngStr('training.plan.trainingPlanFor')} {dateView}{ownerName ? ' (' + ownerName + ')' : ''}</h4>

        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th className="nameColumn">{lngStr('training.exercise.header')}</th>
              {this.state.planDay.percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
              <th className="intColumn text-center">{lngStr('training.entity.liftCounter')}</th>
              <th className="intColumn text-center">{lngStr('training.entity.weightLoad')}</th>
              <th className="intColumn text-center">{lngStr('training.entity.intensity')}</th>
            </tr>
          </thead>
          <tbody>
            {this.state.planDay.exercises.map((planExercise, i) =>
              <tr key={'planTr' + i}>
                <td id={'exerciseName' + i} role="button" onClick={() => this.editSettings(planExercise)}>
                  {planExercise.exercise.name}
                </td>
                <Tooltip text={lngStr('general.actions.schedule')} tooltipTargetId={'exerciseName' + i} placement="left" />

                {this.state.planDay.percentages.map(item =>
                  <td key={item.id} className="text-center">
                    <ExerciseSettingsEditPanel percentage={item} planExercise={planExercise} />
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
              <td><i>{lngStr('training.entity.liftCounterByZones')}</i></td>
              {this.state.planDay.counters.liftIntensities.map((intensity, i) =>
                <td key={'kph' + i} className="text-center"> {intensity.value} </td>
              )}
              <td className="text-center"><strong>{this.state.planDay.counters.liftCounterSum}</strong></td>
              <td className="text-center"><strong>{this.state.planDay.counters.weightLoadSum}</strong></td>
              <td className="text-center"><strong>{this.state.planDay.counters.intensitySum}</strong></td>
            </tr>
          </tfoot>
        </table>

        <Button className="spaceRight" color="primary" onClick={() => this.confirmAsync()}>{lngStr('general.actions.confirm')}</Button>
        <Button color="primary" outline onClick={() => this.props.navigate(`/editPlanExercises/${this.props.params.planId}/${this.props.params.id}`)}>{lngStr('general.actions.back')}</Button>
      </>
    );
  }

}

export default WithRouter(PlanDayEdit);