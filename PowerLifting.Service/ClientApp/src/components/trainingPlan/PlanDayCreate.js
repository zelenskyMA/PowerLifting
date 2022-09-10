import React, { Component } from 'react';
import { GetAsync } from "../../common/ApiActions";
import { Locale } from "../../common/Localization";
import { PanelPlanDay } from "./PanelPlanDay";
import WithRouter from "../../common/extensions/WithRouter";

class PlanDayCreate extends Component {
  constructor() {
    super();

    this.state = {
      planDay: Object,
      percentages: [],
      loading: true,
    };
  }

  componentDidMount() { this.loadPlanDay(); }

  async loadPlanDay() {
    var data = await GetAsync(`trainingPlan/getPlanDay?dayId=${this.props.params.id}`);
    var percentages = await GetAsync("exercise/getPercentages");
    this.setState({ planDay: data, percentages: percentages, loading: false });
  }

  openSettings = (settings) => { this.props.navigate(`/editPlanExerciseSettings/${this.props.params.id}/${settings.id}`); }

  render() {
    var dateView = (new Date(this.state.planDay.activityDate)).toLocaleString(Locale, { dateStyle: "medium" });

    return (
      <>
        <h2 style={{ marginBottom: '30px' }}>План тренировок на {dateView}</h2>
        {this.state.loading ?
          <p><em>Загрузка...</em></p> :
          <PanelPlanDay planDay={this.state.planDay} percentages={this.state.percentages} rowDblClick={this.openSettings} />}
      </>
    );
  }

}

export default WithRouter(PlanDayCreate);