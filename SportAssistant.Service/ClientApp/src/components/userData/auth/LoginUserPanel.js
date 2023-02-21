import React from "react";
import { Link } from 'react-router-dom';
import { Button, Col, Container, NavLink, Row } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputEmail, InputPassword } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { SetToken } from "../../../common/TokenActions";
import '../../../styling/Common.css';
import '../../../styling/Custom.css';

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
    const lngStr = this.props.lngStr;

    return (
      <Container onKeyPress={async (e) => e.key === 'Enter' && await this.onLogin()} fluid>
        <h3 className="first-page-text">{lngStr('appSetup.user.loginToApp')}</h3>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop spaceBottom">
          <Col xs={4}>
            <InputEmail label={lngStr('general.auth.login') + ':'} propName="login" onChange={this.onValueChange} />
          </Col>
          <Col xs={4}>
            <InputPassword label={lngStr('general.auth.password') + ':'} propName="password" onChange={this.onValueChange} />
          </Col>
        </Row>

        <NavLink className="inlineLink first-page-text" tag={Link} to="/register">{lngStr('general.auth.registration')}</NavLink>
        <NavLink className="inlineLink first-page-text" tag={Link} to="/changePassword">{lngStr('general.auth.changePwd')}</NavLink>
        <NavLink className="inlineLink first-page-text" tag={Link} to="/resetPassword">{lngStr('general.auth.resetPwd')}</NavLink>

        <Button className="spaceTop first-page-button" onClick={() => this.onLogin()}>{lngStr('general.actions.enter')}</Button>
      </Container>
    );
  }
}

export default WithRouter(LoginUser)