import React, { Component } from 'react';
import { Button, Row, Col, Label } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { InputNumber, InputText } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class UserCabinet extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userInfo: Object,
      trainingRequest: Object,
      pushAchivement: Object, // толчок, id = 1
      jerkAchivement: Object // рывок, id = 2
    };
  }

  componentDidMount() { this.getUserInfo(); }

  getUserInfo = async () => {
    const [info, achivementsData, trainingRequestData] = await Promise.all([
      GetAsync("/userInfo/get"),
      GetAsync("/userAchivement/get"),
      GetAsync("/trainingRequests/getMyRequest")
    ]);

    var push = achivementsData.find(t => t.exerciseTypeId === 1);
    var jerk = achivementsData.find(t => t.exerciseTypeId === 2);

    this.setState({ userInfo: info, trainingRequest: trainingRequestData, pushAchivement: push, jerkAchivement: jerk });
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ userInfo: { ...prevState.userInfo, [propName]: value } })); }
  onPushChange = (propName, value) => { this.setState(prevState => ({ pushAchivement: { ...prevState.pushAchivement, [propName]: value } })); }
  onJerkChange = (propName, value) => { this.setState(prevState => ({ jerkAchivement: { ...prevState.jerkAchivement, [propName]: value } })); }

  createRequest = () => { this.props.navigate("/coachSelection"); }
  cancelRequest = async () => {
    await PostAsync(`/trainingRequests/remove`);
    var trainingRequestData = await GetAsync("/trainingRequests/getMyRequest");
    this.setState({ trainingRequest: trainingRequestData });
  }

  rejectCoach = async () => {
    await PostAsync(`/trainingGroups/rejectCoach`);
    var info = await GetAsync("/userInfo/get");
    this.setState({ userInfo: info });
  }

  confirmAsync = async () => {
    await PostAsync(`/userInfo/update`, this.state.userInfo);
    await PostAsync(`/userAchivement/create`, [this.state.pushAchivement, this.state.jerkAchivement]);
    this.props.navigate("/");
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Кабинет пользователя</h3>

        <p>Личные данные</p>
        {this.personalInfoPanel()}

        <hr style={{ width: '75%', paddingTop: "2px", marginBottom: '30px' }} />

        <p>Спортивные данные</p>
        {this.sportInfoPanel()}

        <Button className="spaceTop" color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
      </>
    );
  }

  personalInfoPanel() {
    return (
      <>
        <Row style={{ marginTop: '10px', marginBottom: '30px' }}>
          <Col xs={3}>
            <InputText label="Фамилия:" propName="surname" onChange={this.onValueChange} initialValue={this.state.userInfo.surname} />
          </Col>
          <Col xs={3}>
            <InputText label="Имя:" propName="firstName" onChange={this.onValueChange} initialValue={this.state.userInfo.firstName} />
          </Col>
          <Col xs={3}>
            <InputText label="Отчество:" propName="patronimic" onChange={this.onValueChange} initialValue={this.state.userInfo.patronimic} />
          </Col>
        </Row>
        <Row style={{ marginBottom: '30px' }}>
          <Col xs={3}>
            <InputNumber label="Вес:" propName="weight" onChange={this.onValueChange} initialValue={this.state.userInfo.weight} />
          </Col>
          <Col xs={3}>
            <InputNumber label="Рост:" propName="height" onChange={this.onValueChange} initialValue={this.state.userInfo.height} />
          </Col>
          <Col xs={3}>
            <InputNumber label="Возраст:" propName="age" onChange={this.onValueChange} initialValue={this.state.userInfo.age} />
          </Col>
        </Row>
      </>
    );
  }

  sportInfoPanel() {
    return (
      <>
        <Row style={{ marginTop: '10px' }}>
          <Col xs={3}>
            <InputNumber label="Рекорд в толчке:" propName="result" onChange={this.onPushChange} initialValue={this.state.pushAchivement?.result} />
          </Col>
          <Col xs={3}>
            <InputNumber label="Рекорд в рывке:" propName="result" onChange={this.onJerkChange} initialValue={this.state.jerkAchivement?.result} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={6}>
            <Label check>
              <span style={{ marginRight: '20px' }} >Мой тренер:</span>
              {this.coachRequestView()}
            </Label>
          </Col>
        </Row>

      </>
    );
  }

  coachRequestView = () => {
    if (this.state.userInfo.coachLegalName) {
      return (<>
        <strong className="spaceRight" >{this.state.userInfo.coachLegalName}</strong>
        <Button color="primary" onClick={() => this.rejectCoach()}>Отказаться</Button >
      </>);
    }

    if (this.state.trainingRequest.coachName) {
      return (<>
        <strong className="spaceRight" >Заявка подана тренеру {this.state.trainingRequest.coachName}</strong>
        <Button color="primary" onClick={() => this.cancelRequest()}>Отменить</Button >
      </>
      );
    }

    return (<Button color="primary" onClick={() => this.createRequest()}>Выбрать</Button>);
  }

}

export default WithRouter(UserCabinet)