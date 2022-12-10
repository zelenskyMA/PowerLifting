import React, { Component } from 'react';
import { connect } from "react-redux";
import { GetAsync } from "../../common/ApiActions";
import { LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { DateToLocal } from "../../common/Localization";
import { GetToken } from '../../common/TokenActions';
import '../../styling/Common.css';
import '../../styling/Custom.css';
import CoachHomePanel from "../coaching/CoachHomePanel";
import PlanDayViewPanel from "../trainingPlan/view/PlanDayViewPanel";

const mapStateToProps = store => {
  return {
    userInfo: store.app.myInfo,
  }
}

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
        {this.mainPanel()}
      </>
    );
  }

  mainPanel = () => {
    if (this.state.loggedUser === false) { return (this.startScreenPanel()); }
    if (this.state.loading) { return (<LoadingPanel />); }

    if (this.props.userInfo?.coachOnly) { return (<CoachHomePanel userInfo={this.props.userInfo} />); }

    return (
      <>
        <p className="spaceBottom" >План на <strong>{DateToLocal(new Date())}</strong></p>
        {this.state.planDay?.exercises?.length > 0 ? <PlanDayViewPanel planDay={this.state.planDay} /> : <p><em>У вас нет тренировок на сегодня</em></p>}
      </>
    );
  }

  startScreenPanel = () => {
    return (
      <div className="first-page-text first-page-spaceTop">
        <h1 className="spaceBottom">Спортивный ассистент</h1>
        <h3>Войдите в свой кабинет, или создайте нового пользователя чтобы запланировать тренировку</h3>
      </div>);
  }

}

export default WithRouter(connect(mapStateToProps, null)(Home));