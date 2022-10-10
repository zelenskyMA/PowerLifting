import React, { Component } from 'react';
import { GetAsync } from "../common/ApiActions";
import { GetToken } from '../common/TokenActions';
import { PlanDayPanel } from "./trainingPlan/PlanDayPanel";
import { DateToLocal } from "../common/Localization";
import WithRouter from "../common/extensions/WithRouter";
import '../styling/Common.css';

class Home extends Component {
  constructor() {
    super();

    this.state = {
      planDay: Object,
      percentages: [],
      achivements: [],
      loggedUser: false,
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
      GetAsync("/trainingPlan/getCurrentDay"),
      GetAsync("/exerciseInfo/getPlanPercentages"),
      GetAsync("/userAchivement/get")
    ]);

    this.setState({ planDay: data, percentages: percentages, achivements: achivementsData, loggedUser: true, loading: false });
  }


  render() {
    return (
      <>
        <h3>Спортивный ассистент</h3>

        {this.planDayPanel()}
      </>
    );
  }

  planDayPanel = () => {
    if (this.state.loggedUser === false) { return (this.noUserPanel()); }
    if (this.state.loading) { return (<p><em>Загрузка...</em></p>); }

    return (
      <>
        <p className="spaceBottom" >Ваш план на <strong>{DateToLocal(new Date())}</strong></p>

        {this.state.planDay.id ?
          <PlanDayPanel planDay={this.state.planDay} percentages={this.state.percentages} achivements={this.state.achivements} />
          : <p><em>У вас нет тренировок на сегодня</em></p>
        }
      </>
    );
  }

  noUserPanel = () => {
    return (<>
      <p className="spaceBottom" >Ведение тренировок для спортсменов. <strong>{DateToLocal(new Date())}</strong></p>
      <p>Войдите в свой кабинет, или создайте нового пользователя чтобы начать планирование тренировок.</p>
    </>);
  }

}

export default WithRouter(Home);