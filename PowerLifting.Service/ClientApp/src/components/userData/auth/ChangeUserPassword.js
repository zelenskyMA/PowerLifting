import React from "react";
import { Container, Button, Row, Col, InputGroupText, InputGroup, Input } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { HandleErrorPanel } from "../../../common/HandlerPanels";
import WithRouter from "../../../common/extensions/WithRouter";

class ChangeUserPassword extends React.Component {

  state = {
    auth: { login: '', password: '', passwordConfirm: '', oldPassword: '' },
    error: ''
  }

  setValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({ auth: { ...prevState.auth, [propName]: val } }));
  }

  onRegister = async () => {
    try {
      await PostAsync(`/user/changePassword`, this.state.auth);
      window.location.replace("/"); //не сохраняем введенные логин/пароль в истории переходов
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    return (
      <>
        <h3>Смена пароля</h3>

        <Container fluid>
          <HandleErrorPanel errorMessage={this.state.error} />

          <Row style={{ marginTop: '25px' }}>
            <Col xs={6}>
              <InputGroup>
                <InputGroupText>Адрес вашей электронной почты (логин):</InputGroupText>
                <Input type="email" onChange={this.setValue('login')} />
              </InputGroup>
            </Col>
          </Row>
          <Row style={{ marginTop: '20px' }}>
            <Col xs={3}>
              <InputGroup>
                <InputGroupText>Старый пароль:</InputGroupText>
                <Input type="password" onChange={this.setValue('oldPassword')} />
              </InputGroup>
            </Col>
          </Row>
          <Row style={{ marginTop: '20px' }}>
            <Col xs={3}>
              <InputGroup>
                <InputGroupText>Новый пароль:</InputGroupText>
                <Input type="password" onChange={this.setValue('password')} />
              </InputGroup>
            </Col>
            <Col xs={3}>
              <InputGroup>
                <InputGroupText>Подтверждение пароля:</InputGroupText>
                <Input type="password" onChange={this.setValue('passwordConfirm')} />
              </InputGroup>
            </Col>
          </Row>

          <Button color="primary" style={{ marginTop: '25px' }} onClick={() => this.onRegister()}>Сменить пароль</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(ChangeUserPassword)