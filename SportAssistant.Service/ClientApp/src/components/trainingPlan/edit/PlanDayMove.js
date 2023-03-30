import React, { Component } from 'react';
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { connect } from "react-redux";
import { Button } from "reactstrap";
import { DeleteAsync, GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal, DateToUtc, Locale } from "../../../common/LocalActions";
import { changeModalVisibility } from "../../../stores/appStore/appActions";

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class PlanDayMove extends Component {
  constructor() {
    super();

    this.state = {
      planDay: Object,
      selectedInfo: Object,
      loading: true,
      error: '',
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var data = await GetAsync(`/planDay/${this.props.params.id}`);

    var planId = parseInt(this.props.params.planId, 10);
    var dayId = parseInt(this.props.params.id, 10);

    this.setState({ planDay: data, selectedInfo: { planId: planId, id: dayId, targetDate: new Date() }, loading: false });
  }

  goBack = () => { this.props.navigate(`/editPlanDays/${this.props.params.planId}`); }

  onSelectionChange = (propName, value) => { this.setState(prevState => ({ error: '', selectedInfo: { ...prevState.selectedInfo, [propName]: value } })); }

  onMove = async () => {
    try {
      var request = this.state.selectedInfo;
      request.targetDate = DateToUtc(this.state.selectedInfo.targetDate);

      await PostAsync("/planDay/move", request);
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onClear = async () => {
    try {
      await DeleteAsync(`/planDay/${this.state.selectedInfo.id}`);
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onAction = async (text, action, lngStr) => {
    var modalInfo = {
      isVisible: true,
      headerText: lngStr('appSetup.modal.confirm'),
      buttons: [{ name: lngStr('general.actions.confirm'), onClick: action, color: "success" }],
      body: () => { return (<p>{text}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;
    var dateView = DateToLocal(this.state.planDay.activityDate);

    return (
      <>
        <h4>{lngStr('training.plan.change')} {dateView}</h4>
        <p>{lngStr('training.transferOrCancel')}</p>
        <ErrorPanel errorMessage={this.state.error} />

        <p className="spaceTop">{lngStr('training.transferToDay')}</p>
        <Calendar onChange={(date) => this.onSelectionChange('targetDate', date)} value={this.state.selectedInfo.targetDate} locale={Locale} />

        <div className="spaceTop">
          <Button color="primary" className="spaceRight" onClick={() => this.onAction(lngStr('training.confirmTransfer'), this.onMove, lngStr)}>{lngStr('general.actions.confirm')}</Button>
          <Button color="primary" outline onClick={() => this.goBack()}>{lngStr('general.actions.back')}</Button>
        </div>

        <div className="spaceMinTop">
          <Button color="primary" className="spaceRight" onClick={() => this.onAction(lngStr('training.plan.confirmPlanDayDeletion'), this.onClear, lngStr)}>{lngStr('training.plan.deletePlanDay')}</Button>
        </div>
      </>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(PlanDayMove));