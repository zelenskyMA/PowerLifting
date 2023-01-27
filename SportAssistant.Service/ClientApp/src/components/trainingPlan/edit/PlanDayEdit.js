import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel } from "../../../common/controls/CustomControls";
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
    var data = await GetAsync(`/planDay/get?id=${this.props.params.id}`);
    this.setState({ planDay: data, loading: false });
  }

  editSettings = (planExercise) => { this.props.navigate(`/editPlanExerciseSettings/${this.props.params.planId}/${planExercise.id}`); }

  confirmAsync = () => { this.props.navigate(`/editPlanDays/${this.props.params.planId}`); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;
    var dateView = DateToLocal(this.state.planDay.activityDate);

    return (
      <>
        <h4 className="spaceBottom">{lngStr('training.trainingPlanFor')} {dateView}</h4>

        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th className="nameColumn">{lngStr('training.exercise')}</th>
              {this.state.planDay.percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
              <th className="intColumn text-center">{lngStr('training.liftCounter')}</th>
              <th className="intColumn text-center">{lngStr('training.weightLoad')}</th>
              <th className="intColumn text-center">{lngStr('training.intensity')}</th>
            </tr>
          </thead>
          <tbody>
            {this.state.planDay.exercises.map((planExercise, i) =>
              <tr key={'planTr' + i}>
                <td role="button" title={lngStr('training.planIt')} onClick={() => this.editSettings(planExercise)}>
                  {planExercise.exercise.name}
                </td>

                {this.state.planDay.percentages.map(item =>
                  <td key={item.id} className="text-center">
                    <ExerciseSettingsEditPanel percentage={item} settings={planExercise.settings} />
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
              <td><i>{lngStr('training.liftCounterByZones')}</i></td>
              {this.state.planDay.liftIntensities.map((intensity, i) =>
                <td key={'kph' + i} className="text-center"> {intensity.value} </td>
              )}
              <td className="text-center"><strong>{this.state.planDay.liftCounterSum}</strong></td>
              <td className="text-center"><strong>{this.state.planDay.weightLoadSum}</strong></td>
              <td className="text-center"><strong>{this.state.planDay.intensitySum}</strong></td>
            </tr>
          </tfoot>
        </table>

        <Button className="spaceRight" color="primary" onClick={() => this.confirmAsync()}>{lngStr('button.confirm')}</Button>
        <Button color="primary" outline onClick={() => this.props.navigate(`/editPlanExercises/${this.props.params.planId}/${this.props.params.id}`)}>{lngStr('button.back')}</Button>
      </>
    );
  }

}

export default WithRouter(PlanDayEdit);