import React from "react";
import { Button, Col, Row, Container } from "reactstrap";
import { PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputEmail } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class ResetUserPassword extends React.Component {

  state = {
    auth: { login: '' },
    error: ''
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', auth: { ...prevState.auth, [propName]: value } })); }

  onReset = async () => {
    try {
      await PostAsync(`/user/resetPassword`, this.state.auth);
      window.location.replace("/"); //не сохраняем введенные логин/пароль в истории переходов
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    return (
      <>
        <h3 className="first-page-text">Восстановление доступа</h3>
        <Container fluid>
          <h6 className="first-page-text">Для восстановления доступа к учетной записи укажите ваш логин.</h6>
          <h6 className="first-page-text">После подтверждения, на ваш электронный адрес будет выслан новый пароль.</h6>

          <ErrorPanel errorMessage={this.state.error} />

          <Row className="spaceTop">
            <Col xs={6}>
              <InputEmail label="Адрес вашей электронной почты (логин):" propName="login" onChange={this.onValueChange} />
            </Col>
          </Row>

          <Button className="spaceTop first-page-button" onClick={() => this.onReset()}>Продолжить</Button>
        </Container>
      </>
    );
  }
}

export default WithRouter(ResetUserPassword)