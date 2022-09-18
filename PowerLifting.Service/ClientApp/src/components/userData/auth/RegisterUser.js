import React from "react";
import { Container, Button, Row, Col } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { SetToken } from "../../../common/TokenActions";
import { ErrorPanel, InputPassword, InputEmail } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";

class RegisterUser extends React.Component {

  state = {
    auth: { login: '', password: '', passwordConfirm: '' },
    error: ''
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ auth: { ...prevState.auth, [propName]: value } })); }

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
          <ErrorPanel errorMessage={this.state.error} />

          <Row style={{ marginTop: '30px' }}>
            <Col xs={6}>
              <InputEmail label="Адрес вашей электронной почты (логин):" propName="login" onChange={this.onValueChange} />
            </Col>
          </Row>
          <Row style={{ marginTop: '30px' }}>
            <Col xs={3}>
              <InputPassword label="Пароль:" propName="password" onChange={this.onValueChange} />
            </Col>
            <Col xs={3}>
              <InputPassword label="Подтверждение пароля:" propName="passwordConfirm" onChange={this.onValueChange} />
            </Col>
          </Row>

          <Button color="primary" style={{ marginTop: '30px' }} onClick={() => this.onRegister()}>Зарегистрироваться</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(RegisterUser)