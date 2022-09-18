import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { DateToLocal } from "../../../common/Localization";
import { PlanDayPanel } from "../PlanDayPanel";
import WithRouter from "../../../common/extensions/WithRouter";

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
    const [data, percentages, achivementsData] = await Promise.all([
      GetAsync(`trainingPlan/getPlanDay?dayId=${this.props.params.id}`),
      GetAsync("exercise/getPercentages"),
      GetAsync(`userAchivement/get`)
    ]);

    this.setState({ planDay: data, percentages: percentages, achivements: achivementsData, loading: false });
  }

  confirmAsync = () => { this.props.navigate("/createPlanDays"); }

  openSettings = (settings) => { this.props.navigate(`/editPlanExerciseSettings/${this.props.params.id}/${settings.id}`); }

  render() {
    var dateView = DateToLocal(this.state.planDay.activityDate);

    return (
      <>
        <h3 style={{ marginBottom: '30px' }}>План тренировок на {dateView}</h3>
        {this.state.loading ?
          <p><em>Загрузка...</em></p> :
          <PlanDayPanel planDay={this.state.planDay} percentages={this.state.percentages} achivements={this.state.achivements}
            rowClick={this.openSettings} mode="Edit" />
        }

        <Button style={{ marginTop: '40px' }} color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
      </>
    );
  }

}

export default WithRouter(PlanDayCreate);