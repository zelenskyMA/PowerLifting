import React from "react";
import { Container, Button, Row, Col, NavLink } from "reactstrap";
import { Link } from 'react-router-dom';
import { PostAsync } from "../../../common/ApiActions";
import { SetToken } from "../../../common/AuthActions";
import { ErrorPanel, InputEmail, InputPassword } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";

class LoginUser extends React.Component {

  state = {
    auth: { login: '', password: '' },
    error: ''
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ auth: { ...prevState.auth, [propName]: value } })); }

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

        <Container fluid>
          <ErrorPanel errorMessage={this.state.error} />

          <Row style={{ marginTop: '30px' }}>
            <Col xs={3}>
              <InputEmail label="Логин:" propName="login" onChange={this.onValueChange} />
            </Col>
            <Col xs={3}>
              <InputPassword label="Пароль:" propName="password" onChange={this.onValueChange} />
            </Col>
          </Row>

          <NavLink style={{ width: '160px', marginLeft: '-15px', marginTop: '10px' }} tag={Link} to="/register">Зарегистрироваться</NavLink>
          <NavLink style={{ width: '160px', marginLeft: '-15px', }} tag={Link} to="/changePassword">Сменить пароль</NavLink>

          <Button color="primary" style={{ marginTop: '30px' }} onClick={() => this.onLogin()}>Войти</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(LoginUser)