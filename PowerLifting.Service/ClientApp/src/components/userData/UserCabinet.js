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
      achivements: [],
    };
  }

  componentDidMount() { this.getUserInfo(); }

  getUserInfo = async () => {
    const [info, achivementsData] = await Promise.all([
      GetAsync("/userInfo/get"),
      GetAsync("/userAchivement/get")
    ]);

    this.setState({ userInfo: info, achivements: achivementsData });
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ userInfo: { ...prevState.userInfo, [propName]: value } })); }

  getAchivement = (typeId) => {
    var achivement = this.state.achivements.find(t => t.exerciseTypeId === typeId);
    if (achivement == null) {
      return 0;
    }

    return achivement.result;
  }

  setAchivement = (typeId) => (event) => {
    var validation = event.target.validity.valid;
    if (!validation) {
      event.preventDefault();
      return;
    }

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
            <InputGroup>
              <InputGroupText>Рекорд в толчке:</InputGroupText>
              <Input pattern="[0-9]*" onChange={this.setAchivement(1)} value={this.getAchivement(1)} />
            </InputGroup>
          </Col>
          <Col xs={3}>
            <InputGroup>
              <InputGroupText>Рекорд в рывке:</InputGroupText>
              <Input pattern="[0-9]*" onChange={this.setAchivement(2)} value={this.getAchivement(2)} />
            </InputGroup>
          </Col>
        </Row>
      </>
    );
  }

}

export default WithRouter(UserCabinet)