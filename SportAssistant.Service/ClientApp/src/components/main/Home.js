import React, { Component } from 'react';
import { connect } from "react-redux";
import { GetAsync } from "../../common/ApiActions";
import { LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { DateToLocal } from "../../common/LocalActions";
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
    const lngStr = this.props.lngStr;

    return (
      <>        
        {this.mainPanel(lngStr)}
      </>
    );
  }

  mainPanel = (lngStr) => {
    if (this.state.loggedUser === false) { return (this.startScreenPanel(lngStr)); }
    if (this.state.loading) { return (<LoadingPanel />); }

    if (this.props.userInfo?.coachOnly) { return (<CoachHomePanel userInfo={this.props.userInfo} />); }

    return (
      <>
        <p className="spaceBottom" >{lngStr('training.plan.for')} <strong>{DateToLocal(new Date())}</strong></p>
        {this.state.planDay?.exercises?.length > 0 ? <PlanDayViewPanel planDay={this.state.planDay} /> : <p><em>{lngStr('training.exercise.nothingForToday')}</em></p>}
      </>
    );
  }

  startScreenPanel = (lngStr) => {
    return (
      <div className="first-page-text first-page-spaceTop">
        <h1 className="spaceBottom">{lngStr('general.appName')}</h1>
        <h3>{lngStr('appSetup.user.loginOrRegister')}</h3>
      </div>);
  }

}

export default WithRouter(connect(mapStateToProps, null)(Home));