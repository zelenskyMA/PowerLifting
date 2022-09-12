import React from "react";
import { Container, Button, Row, Col, InputGroupText, InputGroup, Input } from "reactstrap";
import { PostAsync } from "../../common/ApiActions";
import WithRouter from "../../common/extensions/WithRouter";

class LoginUser extends React.Component {

  state = {
    auth: { login: '', password: '' },
  }

  setValue = (propName) => (event) => {
    var val = event.target.value;
    this.setState(prevState => ({ exercisesSettings: { ...prevState.exercisesSettings, [propName]: val } }));
  }

  onLogin = async () => {
    try {
      await PostAsync(`/user/login`, this.state.exercisesSettings);
      this.props.navigate("/");
    }
    catch (error) {
      alert(error.message);
    }
  }

  render() {
    return (
      <>
        <h1>Вход</h1>

        <Container style={{ marginTop: '25px' }} fluid>
          <Row>
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

          <Button color="primary" style={{ marginTop: '25px' }} onClick={() => this.onLogin()}>Войти</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(LoginUser)