import React, { Component } from 'react';
import { Button, Row, Col, Input, InputGroup, InputGroupText } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";

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
    var achivementData = await GetAsync(`userAchivement/getByExercise?userId=1&exerciseTypeId=${settingsData?.exercise?.exerciseTypeId}`);
    this.setState({ exercisesSettings: settingsData, achivement: achivementData, loading: false });
  }

  confirmAsync = async () => {
    await PostAsync(`/exercise/updateExerciseSettings`, this.state.exercisesSettings);
    this.props.navigate(`/createPlanDay/${this.props.params.dayId}`);
  }

  // Update state object property by name. Other properties left unchanged
  setValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({
      exercisesSettings: { ...prevState.exercisesSettings, [propName]: val }
    }));
  }

  setAreaValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({ exercisesSettings: { ...prevState.exercisesSettings, [propName]: val } }));
  }

  render() {
    if (this.state.loading) {
      return (<p><em>Загрузка...</em></p>);
    }

    return (
      <>
        <h2 >Упражнение '<i>{this.state.exercisesSettings?.exercise?.name}</i>'</h2>

        {this.percentageInfoPanel(this.state.exercisesSettings.percentage, this.state.achivement)}

        <Row style={{ marginBottom: '30px' }}>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Поднимаемый вес:</InputGroupText>
              <Input onChange={this.setValue('weight')} value={this.state.exercisesSettings.weight} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Количество подходов к штанге:</InputGroupText>
              <Input onChange={this.setValue('iterations')} value={this.state.exercisesSettings.iterations} />
            </InputGroup>
          </Col>
        </Row>
        <Row>
          <Col xs={5}>
            <InputGroup>
              <InputGroupText>Количество повторов частей упражнения:</InputGroupText>
              <Input onChange={this.setValue('exercisePart1')} value={this.state.exercisesSettings.exercisePart1} />
              <Input onChange={this.setValue('exercisePart2')} value={this.state.exercisesSettings.exercisePart2} />
              <Input onChange={this.setValue('exercisePart3')} value={this.state.exercisesSettings.exercisePart3} />
            </InputGroup>
          </Col>
        </Row>

        <p style={{ marginTop: '40px' }}>Оставьте комментарий для выполняющего упражнение: </p>
        <textarea onChange={this.setAreaValue('comments')} value={this.state?.exercisesSettings?.comments} rows={3} cols={84} />

        <br />
        <Button style={{ marginTop: '40px' }} color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
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
          <Col xs={3}>Ожидаемое значение веса: {(achivement.result) / 100 * percentage.minValue} - {(achivement.result) / 100 * percentage.maxValue}</Col>
        </Row>
      </>
    );
  }

}


export default WithRouter(PlanExerciseSettingsEdit);