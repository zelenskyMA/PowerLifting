import React, { Component } from 'react';
import { Col, Row, Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { ErrorPanel, InputNumber, InputText, InputCheckbox } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class UserAdministrationPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userSearch: { id: 0, login: '' },
      blockUser: { block: false, reason: '' },
      userRoles: { admin: false, coach: false },

      userCard: Object,
      error: '',
    };
  }

  onSearchChange = (propName, value) => { this.setState(prevState => ({ userSearch: { ...prevState.userSearch, [propName]: value } })); }
  onBlockChange = (propName, value) => { this.setState(prevState => ({ blockUser: { ...prevState.blockUser, [propName]: value } })); }
  onUserRoleChange = (propName, value) => { this.setState(prevState => ({ userRoles: { ...prevState.userRoles, [propName]: value } })); }

  onUserSearch = async () => {
    try {
      var cardData = await GetAsync(`/userInfo/getCard?userId=${this.state.userSearch.id}&login=${this.state.userSearch.login}`);

      var blockReason = this.state.userCard?.blockReason?.reason == null ? '' : this.state.userCard?.blockReason?.reason;

      this.setState({
        userCard: cardData,
        blockUser: { block: blockReason == '', reason: blockReason },
        error: ''
      });
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  blockUser = async () => {
    try {
      var blockReason = this.state.userCard?.blockReason?.reason == null ? '' : this.state.userCard?.blockReason?.reason;
      var flag = blockReason === '';

      if (blockReason === this.state.blockUser.reason && flag === this.state.blockUser.block) { return; }

      var blockInfo = {
        userId: this.state.userCard?.userId,
        status: this.state.blockUser.block,
        reason: this.state.blockUser.reason
      }

      await PostAsync('/administration/applyBlock', blockInfo);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  applyUserRoles = async () => {
    try {
      var roleInfo = {
        userId: this.state.userCard?.userId,
        admin: this.state.userRoles.admin,
        coach: this.state.userRoles.coach
      }

      await PostAsync('/administration/applyRoles', roleInfo);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    return (
      <>
        <p className="spaceTop">Поиск пользователя</p>
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={3}>
            <InputNumber label="По идентификатору" initialValue={this.state.userSearch.id} propName="id" onChange={this.onSearchChange} />
          </Col>
          <Col xs={3}>
            <InputText label="По логину" initialValue={this.state.userSearch.login} propName="login" onChange={this.onSearchChange} />
          </Col>
          <Col xs={3}>
            <Button color="primary" onClick={() => this.onUserSearch()}>Поиск</Button>
          </Col>
        </Row>
        <hr style={{ width: '80%', paddingTop: "2px" }} />

        {this.userData()}
      </>
    );
  }

  userData = () => {
    if (this.state.userCard?.Id == null) { return (<></>); }

    return (
      <>
        <h6 className="spaceTop">Найден пользователь {this.state.userCard?.baseInfo?.legalName}</h6>
        <Row>
          <Col xs={3}>
            Ид: {this.state.userCard?.userId}
          </Col>
          <Col xs={3}>
            Логин: {this.state.userCard?.login}
          </Col>
        </Row>

        <h6 className="spaceTop">Блокировка</h6>
        <Row>
          <Col xs={2}>
            <InputCheckbox label="Заблокирован" initialValue={this.state.userCard?.blockReason == null} propName="block" onChange={this.onBlockChange} />
          </Col>
          <Col xs={6}>
            <InputText label="Причина" initialValue={this.state.userCard?.blockReason?.reason} propName="reason" onChange={this.onBlockChange} />
          </Col>
          <Col xs={3}>
            <Button color="primary" onClick={() => this.blockUser()}>Применить</Button>
          </Col>
        </Row>

        <h6 className="spaceTop">Роли пользователя</h6>
        <Col xs={3}>
          <InputCheckbox label="Тренер" initialValue={this.state.userCard?.blockReason == null} propName="coach" onChange={this.onUserRoleChange} />
        </Col>
        <Col xs={3}>
          <InputCheckbox label="Администратор" initialValue={this.state.userCard?.blockReason == null} propName="admin" onChange={this.onUserRoleChange} />
        </Col>
        <Col xs={3}>
          <Button color="primary" onClick={() => this.applyUserRoles()}>Применить</Button>
        </Col>
      </>
    );
  }

}

export default WithRouter(UserAdministrationPanel)