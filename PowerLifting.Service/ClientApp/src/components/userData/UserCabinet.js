import React, { Component } from 'react';
import { Button, Row, Col, Input, InputGroup, InputGroupText } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";

class UserCabinet extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userInfo: Object,
      achivements: [],
    };
  }

  componentDidMount() { this.getUserInfo(); }

  getUserInfo = async () => {
    var info = await GetAsync("/userInfo/get");
    var achivementsData = await GetAsync(`userAchivement/get`);
    this.setState({ userInfo: info, achivements: achivementsData });
  }

  setValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({ userInfo: { ...prevState.userInfo, [propName]: val } }));
  }

  getAchivement = (typeId) => {
    var achivement = this.state.achivements.find(t => t.exerciseTypeId === typeId);
    if (achivement == null) {
      return 0;
    }

    return achivement.result;
  }

  setAchivement = (typeId) => (event) => {
    var val = event.target.value;
    var achivements = this.state.achivements;

    var achivement = achivements.find(t => t.exerciseTypeId === typeId);
    if (achivement == null) {
      achivements = [...achivements, { exerciseTypeId: typeId, result: val }];
    }
    else {
      achivement.result = val;
    }

    this.setState({ achivements: achivements });
  }

  confirmAsync = async () => {
    await PostAsync(`/userInfo/update`, this.state.userInfo);
    await PostAsync(`/userAchivement/create`, this.state.achivements);
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
            <InputGroup>
              <InputGroupText>Имя:</InputGroupText>
              <Input onChange={this.setValue('firstName')} value={this.state.userInfo.firstName} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Фамилия:</InputGroupText>
              <Input onChange={this.setValue('surname')} value={this.state.userInfo.surname} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Отчество:</InputGroupText>
              <Input onChange={this.setValue('patronimic')} value={this.state.userInfo.patronimic} />
            </InputGroup>
          </Col>
        </Row>
        <Row style={{ marginBottom: '30px' }}>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Вес:</InputGroupText>
              <Input onChange={this.setValue('weight')} value={this.state.userInfo.weight} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Рост:</InputGroupText>
              <Input onChange={this.setValue('height')} value={this.state.userInfo.height} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Возраст:</InputGroupText>
              <Input onChange={this.setValue('age')} value={this.state.userInfo.age} />
            </InputGroup>
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
            <InputGroup>
              <InputGroupText>Рекорд в толчке:</InputGroupText>
              <Input onChange={this.setAchivement(1)} value={this.getAchivement(1)} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Рекорд в рывке:</InputGroupText>
              <Input onChange={this.setAchivement(2)} value={this.getAchivement(2)} />
            </InputGroup>
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(UserCabinet)