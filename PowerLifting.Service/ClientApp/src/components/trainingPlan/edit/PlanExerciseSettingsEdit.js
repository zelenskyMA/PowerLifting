import React, { Component } from 'react';
import { Button, Row, Col } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { WeightCount } from "../../../common/Localization";
import { InputNumber, InputTextArea, MultiNumberInput } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";

class PlanExerciseSettingsEdit extends Component {
  constructor() {
    super();

    this.state = {
      exercisesSettings: Object,
      achivement: Object,
      loading: true
    };
  }

  componentDidMount() { this.loadExerciseSettings(); }

  async loadExerciseSettings() {
    var settingsData = await GetAsync(`exercise/getExerciseSettings?id=${this.props.params.id}`);
    var achivementData = await GetAsync(`userAchivement/getByExercise?exerciseTypeId=${settingsData?.exercise?.exerciseTypeId}`);
    this.setState({ exercisesSettings: settingsData, achivement: achivementData, loading: false });
  }

  confirmAsync = async () => {
    await PostAsync(`/exercise/updateExerciseSettings`, this.state.exercisesSettings);
    this.props.navigate(`/createPlanDay/${this.props.params.dayId}`);
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ exercisesSettings: { ...prevState.exercisesSettings, [propName]: value } })); }

  render() {
    if (this.state.loading) {
      return (<p><em>Загрузка...</em></p>);
    }

    return (
      <>
        <h3 >Упражнение '<i>{this.state.exercisesSettings?.exercise?.name}</i>'</h3>

        {this.percentageInfoPanel(this.state.exercisesSettings.percentage, this.state.achivement)}

        <Row style={{ marginTop: '30px' }}>
          <Col xs={3}>
            <InputNumber label="Вес:" propName="weight" onChange={this.onValueChange} initialValue={this.state.exercisesSettings.weight} />
          </Col>
          <Col xs={3}>
            <InputNumber label="Количество подходов:" propName="iterations"
              onChange={this.onValueChange} initialValue={this.state.exercisesSettings.iterations} />
          </Col>
        </Row>
        <Row style={{ marginTop: '30px' }}>
          <Col xs={5}>
            <MultiNumberInput label="Количество повторов частей упражнения:" onChange={this.onValueChange} inputList={[
              { propName: 'exercisePart1', initialValue: this.state.exercisesSettings.exercisePart1 },
              { propName: 'exercisePart2', initialValue: this.state.exercisesSettings.exercisePart2 },
              { propName: 'exercisePart3', initialValue: this.state.exercisesSettings.exercisePart3 }]
            } />
          </Col>
        </Row>

        <Row style={{ marginTop: '30px' }}>
          <Col xs={5}>
            <InputTextArea onChange={this.onValueChange} propName="comments" cols={85}
              label="Оставьте комментарий для выполняющего упражнение:" initialValue={this.state?.exercisesSettings?.comments} />
          </Col>
        </Row>

        <Button color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
      </>
    );
  }

  percentageInfoPanel(percentage, achivement) {

    return (
      <>
        <Row>
          <Col>
            Интенсивность тренировки в процентах <i>{percentage.name}</i>.
          </Col>
        </Row>
        <Row style={{ marginTop: '5px', marginBottom: '25px' }}>
          <Col xs={2}>Ваш рекорд: {achivement.result}</Col>
          <Col xs={3}>
            Ожидаемое значение веса: {WeightCount(achivement?.result, percentage?.minValue)} - {WeightCount(achivement?.result, percentage?.maxValue)}
          </Col>
        </Row>
      </>
    );
  }

}


export default WithRouter(PlanExerciseSettingsEdit);