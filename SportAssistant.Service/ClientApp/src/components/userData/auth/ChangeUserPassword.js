import React from "react";
import { Button, Col, Container, Row } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputEmail, InputPassword } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import '../../../styling/Custom.css';

class ChangeUserPassword extends React.Component {

  state = {
    auth: { login: '', password: '', passwordConfirm: '', oldPassword: '' },
    error: ''
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', auth: { ...prevState.auth, [propName]: value } })); }

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
    const lngStr = this.props.lngStr;

    return (
      <>
        <h3 className="first-page-text">{lngStr('auth.changePwd')}</h3>

        <Container fluid>
          <ErrorPanel errorMessage={this.state.error} />

          <Row className="spaceTop">
            <Col xs={9}>
              <InputEmail label={lngStr('auth.address')} propName="login" onChange={this.onValueChange} />
            </Col>
          </Row>
          <Row className="spaceTop">
            <Col xs={4}>
              <InputPassword label={lngStr('auth.oldPwd')} propName="oldPassword" onChange={this.onValueChange} />
            </Col>
          </Row>
          <Row className="spaceTop">
            <Col xs={4}>
              <InputPassword label={lngStr('auth.newPwd')} propName="password" onChange={this.onValueChange} />
            </Col>
            <Col xs={5}>
              <InputPassword label={lngStr('auth.confirmPwd')} propName="passwordConfirm" onChange={this.onValueChange} />
            </Col>
          </Row>

          <Button className="spaceTop first-page-button" onClick={() => this.onRegister()}>{lngStr('button.confirm')}</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(ChangeUserPassword)