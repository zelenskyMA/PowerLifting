import React, { Component } from 'react';
import { Button, Row, Col } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { DropdownControl, InputText, InputTextArea, ErrorPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class ExerciseEdit extends Component {
  constructor() {
    super();

    this.state = {
      exercise: Object,
      types: [],
      subTypes: [],
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    const [exerciseData, typesData, subTypesData] = await Promise.all([
      GetAsync(`exerciseInfo/get?id=${this.props.params.id ?? 0}`),
      GetAsync(`dictionary/getListByType?id=1`),
      GetAsync(`dictionary/getListByType?id=2`)
    ]);

    this.setState({ exercise: exerciseData, types: typesData, subTypes: subTypesData, loading: false });
  }

  onConfirm = async () => {
    try {
      await PostAsync(`/exerciseInfo/updateExercise`, this.state.exercise);
      this.props.navigate(`/exercises`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onDelete = async () => {
    try {
      await PostAsync(`/exerciseInfo/deleteExercise?Id=${this.state.exercise.id}`);
      this.props.navigate(`/exercises`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onTypeSelect = (id) => { this.onValueChange("exerciseTypeId", id); }
  onSubtypeSelect = (id) => { this.onValueChange("exerciseSubTypeId", id); }

  onValueChange = (propName, value) => {
    this.setState({ error: '' });
    this.setState(prevState => ({ exercise: { ...prevState.exercise, [propName]: value } }));
  }

  render() {
    if (this.state.loading) { return (<p><em>Загрузка...</em></p>); }

    return (
      <>
        <h4><i>{this.state.exercise?.name}</i></h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={3}>
            <InputText label="Название:" propName="name" onChange={this.onValueChange} initialValue={this.state.exercise.name} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={3}>
            <DropdownControl placeholder="Не задано" label="Тип упражнения: " defaultValue={this.state.exercise.exerciseTypeId}
              data={this.state.types} onChange={this.onTypeSelect} />
          </Col>
          <Col xs={5}>
            <DropdownControl placeholder="Не задано" label="Подтип упражнения: " defaultValue={this.state.exercise.exerciseSubTypeId}
              data={this.state.subTypes} onChange={this.onSubtypeSelect} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={5}>
            <InputTextArea onChange={this.onValueChange} propName="description" cols={95}
              label="Описание упражнения" initialValue={this.state.exercise.description} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={3}>
            <Button color="primary" onClick={() => this.onConfirm()}>Подтвердить</Button>
          </Col>
          <Col xs={1}>
            <Button color="primary" onClick={() => this.onDelete()}>Удалить</Button>
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(ExerciseEdit);