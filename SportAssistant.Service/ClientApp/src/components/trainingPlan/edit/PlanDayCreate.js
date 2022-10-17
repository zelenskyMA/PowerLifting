import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal } from "../../../common/Localization";
import '../../../styling/Common.css';
import { ExerciseSettingsEditPanel } from "./ExerciseSettingsEditPanel";

class PlanDayCreate extends Component {
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

  editSettings = (planExercise) => { this.props.navigate(`/editPlanExerciseSettings/${planExercise.id}`); }

  confirmAsync = () => { this.props.navigate("/createPlanDays"); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    var dateView = DateToLocal(this.state.planDay.activityDate);

    return (
      <>
        <h3 className="spaceBottom">План тренировок на {dateView}</h3>

        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th className="nameColumn" >Упражнение</th>
              {this.state.planDay.percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
              <th className="intColumn text-center">КПШ</th>
              <th className="intColumn text-center">Нагрузка</th>
              <th className="intColumn text-center">Интенсивность</th>
            </tr>
          </thead>
          <tbody>
            {this.state.planDay.exercises.map((planExercise, i) =>
              <tr key={'planTr' + i}>
                <td role="button" title="Запланировать" onClick={() => this.editSettings(planExercise)}>
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
              <td><i>КПШ по зонам интенсивности</i></td>
              {this.state.planDay.liftIntensities.map((intensity, i) =>
                <td key={'kph' + i} className="text-center"> {intensity.value} </td>
              )}
              <td className="text-center"><strong>{this.state.planDay.liftCounterSum}</strong></td>
              <td className="text-center"><strong>{this.state.planDay.weightLoadSum}</strong></td>
              <td className="text-center"><strong>{this.state.planDay.intensitySum}</strong></td>
            </tr>
          </tfoot>
        </table>

        <Button className="spaceRight" color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
        <Button color="primary" outline onClick={() => this.props.navigate(`/createPlanExercises/${this.props.params.id}`)}>Назад</Button>
      </>
    );
  }

}

export default WithRouter(PlanDayCreate);