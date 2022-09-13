import React from "react";
import { Container, Button, Row, Col, InputGroupText, InputGroup, Input } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { SetToken } from "../../../common/AuthActions";
import { HandleErrorPanel } from "../../../common/HandlerPanels";
import WithRouter from "../../../common/extensions/WithRouter";

class RegisterUser extends React.Component {

  state = {
    auth: { login: '', password: '', passwordConfirm: '' },
    error: ''
  }

  setValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({ auth: { ...prevState.auth, [propName]: val } }));
  }

  onRegister = async () => {
    try {
      var tokenData = await PostAsync(`/user/register`, this.state.auth);
      SetToken(tokenData);

      window.location.replace("/"); //не сохраняем введенные логин/пароль в истории переходов
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    return (
      <>
        <h3>Регистрация</h3>

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
                <InputGroupText>Пароль:</InputGroupText>
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

          <Button color="primary" style={{ marginTop: '25px' }} onClick={() => this.onRegister()}>Зарегистрироваться</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(RegisterUser)