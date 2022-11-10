import React from "react";
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal, Locale } from "../../../common/Localization";
import { changeModalVisibility } from "../../../stores/appStore/appActions";

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class PlanDaysEdit extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      plannedDays: [],
      typeCounters: [],
      backUrl: '',
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var plan = await GetAsync(`/trainingPlan/get?Id=${this.props.params.planId}`);
    var url = plan.isMyPlan ? `/plansList` : `/groupUser/${plan.userId}`;

    this.setState({ plannedDays: plan.trainingDays, typeCounters: plan.typeCountersSum, backUrl: url });
  }

  onSetExercises = (dayId) => { this.props.navigate(`/editPlanExercises/${this.props.params.planId}/${dayId}`); }

  onConfirmDelete = async () => {
    try {
      await PostAsync("/trainingPlan/delete", { id: this.props.params.planId });
      this.props.navigate(this.state.backUrl);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onDeletePlan = async () => {
    var modalInfo = {
      isVisible: true,
      headerText: "Запрос подтверждения",
      buttons: [{ name: "Подтвердить", onClick: this.onConfirmDelete, color: "success" }],
      body: () => { return (<p>Подтвердите удаление плана</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    const days = this.state.plannedDays;
    const placeHolder = "Не задано";

    return (
      <>
        <h4>Запланированные дни тренировок</h4>
        <br />
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={3} md={{ offset: 4 }}><strong>Назначьте упражнения на дни недели.</strong></Col>
        </Row>
        <br />
        <Container fluid>
          <Row>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[0])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[1])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[2])}</Col>
          </Row>
          <Row style={{ marginTop: '100px' }}>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[3])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[4])}</Col>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[5])}</Col>
          </Row>
          <Row style={{ marginTop: '100px', marginBottom: '50px' }}>
            <Col>{days.length === 0 ? placeHolder : this.dayPanel(days[6])}</Col>

            <Col>
              <div><strong>Всего упражнений</strong></div>
              {this.countersPanel(this.state.typeCounters)}
            </Col>

            <Col>{this.buttonPanel()}</Col>
          </Row>
        </Container>
      </>
    );
  }

  dayPanel(day) {
    var dateName = new Date(day.activityDate).toLocaleString(Locale, { weekday: 'long' });
    dateName = dateName.charAt(0).toUpperCase() + dateName.slice(1);

    return (
      <Container fluid>
        <Row>
          <Col className="text-center">
            <div><strong>{dateName}</strong></div>
            <div>{DateToLocal(day.activityDate)}</div>
          </Col>
          <Col style={{ paddingTop: '7px' }} >
            <Button color="primary" outline title="Назначить упражнения" style={{ width: '40px',  marginRight: '10px' }} onClick={() => this.onSetExercises(day.id)} >{' + '}</Button>
            <Button color="primary" outline title="Перенести упражнения" style={{ width: '40px'}} disabled={day.exerciseTypeCounters.length == 0} onClick={() => this.props.navigate(`/movePlanDay/${this.props.params.planId}/${day.id}`)} >{' - '}</Button>
          </Col>
        </Row>
        <hr style={{ width: '60%', paddingTop: "2px" }} />
        <Row>
          <Col>{this.countersPanel(day.exerciseTypeCounters)}</Col>
        </Row>
      </Container>
    );
  }

  countersPanel(counters) {
    var fontSize = "0.85rem";

    if (counters.length == 0) {
      return (
        <span style={{ fontSize: fontSize }}>Нет назначенных упражнений</span>
      );
    }

    return (
      <>
        {counters.map((item, i) =>
          <Row key={i} style={{ fontSize: fontSize }}>
            <Col>{item.name}</Col>
            <Col>{item.value}</Col>
          </Row>
        )}
      </>
    );
  }

  buttonPanel() {
    return (
      <Col>
        <Button color="primary" onClick={async () => this.props.navigate(this.state.backUrl)}>Завершить назначение плана</Button>
        <p></p>
        <Button color="primary" outline onClick={async () => this.onDeletePlan()}>Удалить план</Button>
      </Col>
    );
  }

}

export default WithRouter(connect(null, mapDispatchToProps)(PlanDaysEdit))