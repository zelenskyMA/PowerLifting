import React from "react";
import { Link } from 'react-router-dom';
import { Button, Col, Container, NavLink, Row } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputEmail, InputPassword } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { SetToken } from "../../../common/TokenActions";
import '../../../styling/Common.css';

class LoginUser extends React.Component {

  state = {
    auth: { login: '', password: '' },
    error: ''
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', auth: { ...prevState.auth, [propName]: value } })); }

  onLogin = async () => {
    try {
      var tokenData = await PostAsync(`/user/login`, this.state.auth);
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
        <h3>Вход</h3>

        <Container onKeyPress={async (e) => e.key === 'Enter' && await this.onLogin()} fluid>
          <ErrorPanel errorMessage={this.state.error} />

          <Row className="spaceTop spaceBottom">
            <Col xs={3}>
              <InputEmail label="Логин:" propName="login" onChange={this.onValueChange} />
            </Col>
            <Col xs={3}>
              <InputPassword label="Пароль:" propName="password" onChange={this.onValueChange} />
            </Col>
          </Row>

          <NavLink className="inlineLink" tag={Link} to="/register">Зарегистрироваться</NavLink>
          <NavLink className="inlineLink" tag={Link} to="/changePassword">Сменить пароль</NavLink>
          <NavLink className="inlineLink" tag={Link} to="/resetPassword">Забыли пароль?</NavLink>

          <Button color="primary" style={{ marginTop: '30px' }} onClick={() => this.onLogin()}>Войти</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(LoginUser)