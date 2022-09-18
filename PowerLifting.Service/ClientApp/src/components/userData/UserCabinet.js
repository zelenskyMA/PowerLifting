import React, { Component } from 'react';
import { Button, Row, Col, Input, InputGroup, InputGroupText } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { InputNumber, InputText } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";

class UserCabinet extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userInfo: Object,
      pushAchivement: Object, // толчок, id = 1
      jerkAchivement: Object // рывок, id = 2
    };
  }

  componentDidMount() { this.getUserInfo(); }

  getUserInfo = async () => {
    const [info, achivementsData] = await Promise.all([
      GetAsync("/userInfo/get"),
      GetAsync("/userAchivement/get")
    ]);

    var push = achivementsData.find(t => t.exerciseTypeId === 1);
    var jerk = achivementsData.find(t => t.exerciseTypeId === 2);

    this.setState({ userInfo: info, pushAchivement: push, jerkAchivement: jerk });
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ userInfo: { ...prevState.userInfo, [propName]: value } })); }
  onPushChange = (propName, value) => { this.setState(prevState => ({ pushAchivement: { ...prevState.pushAchivement, [propName]: value } })); }
  onJerkChange = (propName, value) => { this.setState(prevState => ({ jerkAchivement: { ...prevState.jerkAchivement, [propName]: value } })); }

  confirmAsync = async () => {
    await PostAsync(`/userInfo/update`, this.state.userInfo);
    await PostAsync(`/userAchivement/create`, [this.state.pushAchivement, this.state.jerkAchivement]);
    this.props.navigate("/");
  }

  render() {
    return (
      <>
        <h3 style={{ marginBottom: '30px' }}>Кабинет пользователя</h3>

        <p>Личные данные</p>
        {this.personalInfoPanel()}

        <hr style={{ width: '75%', paddingTop: "2px", marginBottom: '30px' }} />

        <p>Спортивные данные</p>
        {this.sportInfoPanel()}

        <Button style={{ marginTop: '40px' }} color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
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
            <InputNumber label="Рекорд в толчке:" propName="result" onChange={this.onPushChange} initialValue={this.state.pushAchivement.result} />
          </Col>
          <Col xs={3}>
            <InputNumber label="Рекорд в рывке:" propName="result" onChange={this.onJerkChange} initialValue={this.state.jerkAchivement.result} />
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(UserCabinet)