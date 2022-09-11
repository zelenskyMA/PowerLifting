import React, { Component } from 'react';
import { Button } from "reactstrap";
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
      achivements: [],
      loading: true,
    };
  }

  componentDidMount() { this.loadPlanDay(); }

  async loadPlanDay() {
    var data = await GetAsync(`trainingPlan/getPlanDay?dayId=${this.props.params.id}`);
    var percentages = await GetAsync("exercise/getPercentages");
    var achivementsData = await GetAsync(`userAchivement/get?userId=1`);
    this.setState({ planDay: data, percentages: percentages, achivements: achivementsData, loading: false });
  }

  confirmAsync = () => { this.props.navigate("/createPlanDays"); }

  openSettings = (settings) => { this.props.navigate(`/editPlanExerciseSettings/${this.props.params.id}/${settings.id}`); }

  render() {
    var dateView = (new Date(this.state.planDay.activityDate)).toLocaleString(Locale, { dateStyle: "medium" });

    return (
      <>
        <h2 style={{ marginBottom: '30px' }}>План тренировок на {dateView}</h2>
        {this.state.loading ?
          <p><em>Загрузка...</em></p> :
          <PanelPlanDay planDay={this.state.planDay} percentages={this.state.percentages} achivements={this.state.achivements}
            rowDblClick={this.openSettings} mode="Edit" />
        }

        <Button style={{ marginTop: '40px' }} color="primary" onClick={() => this.confirmAsync()}>Назад</Button>
      </>
    );
  }

}

export default WithRouter(PlanDayCreate);