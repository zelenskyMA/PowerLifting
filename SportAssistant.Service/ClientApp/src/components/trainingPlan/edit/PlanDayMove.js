import React, { Component } from 'react';
import { connect } from "react-redux";
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel } from "../../../common/controls/CustomControls";
import { DateToLocal, DateToUtc, Locale } from "../../../common/LocalActions";
import WithRouter from "../../../common/extensions/WithRouter";
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

  onAction = async (text, action) => {
    var modalInfo = {
      isVisible: true,
      headerText: "Запрос подтверждения",
      buttons: [{ name: "Подтвердить", onClick: action, color: "success" }],
      body: () => { return (<p>{text}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
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

        <div className="spaceTop">
          <Button color="primary" className="spaceRight" onClick={() => this.onAction('Подтвердите перенос тренировки', this.onMove)}>Перенести</Button>
          <Button color="primary" className="spaceRight" onClick={() => this.onAction('Подтвердите отмену', this.onClear)}>Отменить</Button>
          <Button color="primary" outline onClick={() => this.goBack()}>Назад</Button>
        </div>
      </>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(PlanDayMove));