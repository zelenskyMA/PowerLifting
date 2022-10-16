import React, { Component } from 'react';
import { GetAsync } from "../common/ApiActions";
import { GetToken } from '../common/TokenActions';
import { PlanDayViewPanel } from "./trainingPlan/view/PlanDayViewPanel";
import { DateToLocal } from "../common/Localization";
import WithRouter from "../common/extensions/WithRouter";
import '../styling/Common.css';

class Home extends Component {
  constructor() {
    super();

    this.state = {
      planDay: Object,
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

    var data = await GetAsync("/planDay/getCurrent");
    this.setState({ planDay: data, loggedUser: true, loading: false });
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

        {this.state.planDay.id ? <PlanDayViewPanel planDay={this.state.planDay} /> : <p><em>У вас нет тренировок на сегодня</em></p>}
      </>
    );
  }

  noUserPanel = () => {
    return (<>
      <p className="spaceBottom" >Ведение тренировок для спортсменов. <strong>{DateToLocal(new Date())}</strong></p>
      <p>Войдите в свой кабинет, или создайте нового пользователя чтобы запланировать тренировку</p>
    </>);
  }

}

export default WithRouter(Home);