import React, { Component } from 'react';
import { GetAsync } from "../common/ApiActions";
import { GetToken } from '../common/AuthActions';
import { PanelPlanDay } from "./trainingPlan/PanelPlanDay";
import WithRouter from "../common/extensions/WithRouter";

class Home extends Component {
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
    if (GetToken() == null) {
      this.setState({ loading: false });
      return;
    }

    const [data, percentages, achivementsData] = await Promise.all([
      GetAsync("trainingPlan/getCurrentDay"),
      GetAsync("exercise/getPercentages"),
      GetAsync("userAchivement/get")
    ]);

    this.setState({ planDay: data, percentages: percentages, achivements: achivementsData, loading: false });
  }


  render() {
    return (
      <>
        <h1>Помощник спортсмена</h1>
        <p style={{ marginBottom: '30px' }}>Программа для ведения планов тренировок спортсменов</p>

        {this.planDayPanel()}        
      </>
    );
  }

  planDayPanel() {
    if (this.state.loading) { return (<p><em>Загрузка...</em></p>); }
    if (this.state.planDay?.id == null) { return (<p><em>Нет тренировок на сегодня</em></p>); }

    return (<PanelPlanDay planDay={this.state.planDay} percentages={this.state.percentages} achivements={this.state.achivements} />);
  }

}

export default WithRouter(Home);