import React, { Component } from 'react';
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel } from "../../../common/controls/CustomControls";
import { DateToLocal } from "../../../common/Localization";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToUtc, Locale } from "../../../common/Localization";

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
    var data = await GetAsync(`/planDay/get?id=${this.props.params.id}`);

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
      await PostAsync("/planDay/clear", this.state.selectedInfo);
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    var dateView = DateToLocal(this.state.planDay.activityDate);

    return (
      <>
        <h4>Изменение плана тренировок {dateView}</h4>
        <p>Перенос тренировки на другой день или ее отмена.</p>
        <ErrorPanel errorMessage={this.state.error} />

        <p className="spaceTop">Выберите день в текущем плане для переноса тренировок.</p>
        <Calendar onChange={(date) => this.onSelectionChange('targetDate', date)} value={this.state.selectedInfo.targetDate} locale={Locale} />

        <Button color="primary" className="spaceTop spaceRight" onClick={() => this.onMove()}>Перенести</Button>
        <Button color="primary" className="spaceTop spaceRight" onClick={() => this.onClear()}>Отменить</Button>
        <Button color="primary" className="spaceTop" outline onClick={() => this.goBack()}>Назад</Button>
      </>
    );
  }
}

export default WithRouter(PlanDayMove);