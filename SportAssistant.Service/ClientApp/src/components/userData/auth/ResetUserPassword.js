import React from "react";
import { Button, Col, Row } from "reactstrap";
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
        <h4>Восстановление доступа</h4>
        <div>Для восстановления доступа к учетной записи укажите ваш логин.</div>
        <div>После подтверждения, на ваш электронный адрес будет выслан новый пароль.</div>

        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={6}>
            <InputEmail label="Адрес вашей электронной почты (логин):" propName="login" onChange={this.onValueChange} />
          </Col>
        </Row>

        <Button color="primary" className="spaceTop" onClick={() => this.onReset()}>Продолжить</Button>
      </>
    );
  }
}

export default WithRouter(ResetUserPassword)