import React from "react";
import { Container, Button, Row, Col, InputGroupText, InputGroup, Input, NavLink } from "reactstrap";
import { Link } from 'react-router-dom';
import { PostAsync } from "../../../common/ApiActions";
import { HandleErrorPanel } from "../../../common/HandlerPanels";
import WithRouter from "../../../common/extensions/WithRouter";

class LoginUser extends React.Component {

  state = {
    auth: { login: '', password: '' },
    error: ''
  }

  setValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({ auth: { ...prevState.auth, [propName]: val } }));
  }

  onLogin = async () => {
    try {
      await PostAsync(`/user/login`, this.state.auth);
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
          <HandleErrorPanel errorMessage={this.state.error} />

          <Row style={{ marginTop: '25px' }}>
            <Col xs={3}>
              <InputGroup>
                <InputGroupText>Логин:</InputGroupText>
                <Input type="email" onChange={this.setValue('login')} />
              </InputGroup>
            </Col>
            <Col xs={3}>
              <InputGroup>
                <InputGroupText>Пароль:</InputGroupText>
                <Input type="password" onChange={this.setValue('password')} />
              </InputGroup>
            </Col>
          </Row>

          <NavLink style={{ width: '160px', marginLeft: '-15px', marginTop: '10px' }} tag={Link} to="/register">Зарегистрироваться</NavLink>
          <NavLink style={{ width: '160px', marginLeft: '-15px', }} tag={Link} to="/changePassword">Сменить пароль</NavLink>

          <Button color="primary" style={{ marginTop: '20px' }} onClick={() => this.onLogin()}>Войти</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(LoginUser)