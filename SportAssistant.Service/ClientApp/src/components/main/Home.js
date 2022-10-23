import React, { Component } from 'react';
import { GetAsync } from "../../common/ApiActions";
import { LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { DateToLocal } from "../../common/Localization";
import { GetToken } from '../../common/TokenActions';
import '../../styling/Common.css';
import PlanDayViewPanel from "../trainingPlan/view/PlanDayViewPanel";

class Home extends Component {
  constructor() {
    super();

    this.state = {
      planDay: Object,
      loggedUser: false,
      loading: true,
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
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
        <h4>Спортивный ассистент</h4>
        {this.planDayPanel()}
      </>
    );
  }

  planDayPanel = () => {
    if (this.state.loggedUser === false) { return (this.startScreenPanel()); }
    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <p className="spaceBottom" >Ваш план на <strong>{DateToLocal(new Date())}</strong></p>
        {this.state.planDay.id ? <PlanDayViewPanel planDay={this.state.planDay} /> : <p><em>У вас нет тренировок на сегодня</em></p>}
      </>
    );
  }

  startScreenPanel = () => {
    return (<>
      <p className="spaceBottom" >Ведение тренировок для спортсменов. <strong>{DateToLocal(new Date())}</strong></p>
      <p>Войдите в свой кабинет, или создайте нового пользователя чтобы запланировать тренировку</p>
    </>);
  }

}

export default WithRouter(Home);