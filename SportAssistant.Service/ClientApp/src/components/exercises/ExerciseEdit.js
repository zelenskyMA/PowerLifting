import React, { Component } from 'react';
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { DropdownControl, ErrorPanel, InputText, InputTextArea, LoadingPanel } from "../../common/controls/CustomControls";
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
      GetAsync(`dictionary/getListByType?typeId=1`),
      GetAsync(`dictionary/getListByType?typeId=2`)
    ]);

    this.setState({ exercise: exerciseData, types: typesData, subTypes: subTypesData, loading: false });
  }

  onConfirm = async () => {
    try {
      await PostAsync(`/exerciseInfo/update`, this.state.exercise);
      this.props.navigate(`/exercises`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onDelete = async () => {
    try {
      await PostAsync(`/exerciseInfo/delete?Id=${this.state.exercise.id}`);
      this.props.navigate(`/exercises`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onTypeSelect = (id) => { this.onValueChange("exerciseTypeId", id); }
  onSubtypeSelect = (id) => { this.onValueChange("exerciseSubTypeId", id); }
  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', exercise: { ...prevState.exercise, [propName]: value } })); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4><i>{this.state.exercise?.name}</i></h4>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={7}>
            <InputText label={lngStr('common.name') + ': '} propName="name" onChange={this.onValueChange} initialValue={this.state.exercise.name} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={3}>
            <DropdownControl placeholder={lngStr('common.notSet')} label={lngStr('training.exerciseType') + ': '} defaultValue={this.state.exercise.exerciseTypeId}
              data={this.state.types} onChange={this.onTypeSelect} />
          </Col>
          <Col xs={5}>
            <DropdownControl placeholder={lngStr('common.notSet')} label={lngStr('training.exerciseSubType') + ': '} defaultValue={this.state.exercise.exerciseSubTypeId}
              data={this.state.subTypes} onChange={this.onSubtypeSelect} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={5}>
            <InputTextArea onChange={this.onValueChange} propName="description" cols={98}
              label={lngStr('training.exerciseDescription')} initialValue={this.state.exercise.description} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={3}>
            <Button color="primary" onClick={() => this.onConfirm()}>{lngStr('button.confirm')}</Button>
          </Col>
          <Col xs={1}>
            <Button color="primary" onClick={() => this.onDelete()}>{lngStr('button.delete')}</Button>
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(ExerciseEdit);