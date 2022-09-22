import React, { Component } from 'react';
import { Container, Col, Row, Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { UserCardModal } from "../userData/UserCardModal";
import { ErrorPanel, InfoPanel, InputNumber, InputText, InputCheckbox } from "../../common/controls/CustomControls";
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
      userCardView: false,
      error: '',
      success: ''
    };
  }

  onSearchChange = (propName, value) => { this.setState(prevState => ({ userSearch: { ...prevState.userSearch, [propName]: value } })); }
  onBlockChange = (propName, value) => { this.setState(prevState => ({ blockUser: { ...prevState.blockUser, [propName]: value } })); }
  onUserRoleChange = (propName, value) => { this.setState(prevState => ({ userRoles: { ...prevState.userRoles, [propName]: value } })); }

  onUserCardView = (value) => { this.setState({ userCardView: value }); }

  onUserSearch = async () => {
    try {
      var searchId = this.state.userSearch.id > 0 ? `userId=${this.state.userSearch.id}` : '';
      var searchLogin = this.state.userSearch.login ? `login=${this.state.userSearch.login}` : '';
      if (!searchId && !searchLogin) {
        this.setState({ error: "Необходимо указать хотя бы один критерий поиска" });
      }

      var cardData = null;
      if (searchId) {
        cardData = await GetAsync(`/userAdministration/getCard?${searchId}`);
      }
      else {
        if (searchLogin) { cardData = await GetAsync(`/userAdministration/getCard?${searchLogin}`); }
      }

      var blockReason = cardData?.blockReason?.reason == null ? '' : cardData?.blockReason?.reason;
      var roles = cardData?.baseInfo?.rolesInfo;

      this.setState({
        userCard: cardData,
        userRoles: { admin: roles?.isAdmin, coach: roles?.isCoach },
        blockUser: { block: blockReason !== '', reason: blockReason },
        error: '',
        success: ''
      });
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  blockUser = async () => {
    try {
      var blockInfo = {
        userId: this.state.userCard?.userId,
        status: this.state.blockUser.block,
        reason: this.state.blockUser.reason
      }

      await PostAsync('/userAdministration/applyBlock', blockInfo);

      this.setState({ success: 'Блокировка изменена успешно', error: '' });
    }
    catch (error) {
      this.setState({ error: error.message, success: '' });
    }
  }

  applyUserRoles = async () => {
    try {
      var roleInfo = {
        userId: this.state.userCard?.userId,
        isAdmin: this.state.userRoles.admin,
        isCoach: this.state.userRoles.coach
      }

      await PostAsync('/userAdministration/applyRoles', roleInfo);
      this.setState({ success: 'Роли изменены успешно', error: '' });
    }
    catch (error) {
      this.setState({ error: error.message, success: '' });
    }
  }

  render() {
    return (
      <>
        <p className="spaceTop">Поиск пользователя</p>
        <ErrorPanel errorMessage={this.state.error} />
        <InfoPanel infoMessage={this.state.success} />

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
    if (this.state.userCard?.userId == null) { return (<></>); }

    return (
      <>
        <h6 className="spaceTop"> Найден пользователь
          <span id="legalName" role="Button" style={{ marginLeft: '10px', color: 'blue', cursor: 'zoom-in' }}
            onMouseEnter={() => this.onUserCardView(true)} onMouseLeave={() => this.onUserCardView(false)}>
            {this.state.userCard?.baseInfo?.legalName}
          </span>
        </h6>
        <UserCardModal userInfo={this.state.userCard?.baseInfo} targetId="legalName" isOpen={this.state.userCardView} />

        <Container fluid>
          <Row>
            <Col xs={3}>
              Ид: {this.state.userCard?.userId}
            </Col>
            <Col xs={3}>
              Логин: {this.state.userCard?.login}
            </Col>
          </Row>
        </Container>

        <h6 className="spaceTop">Блокировка</h6>
        <Container fluid>
          <Row>
            <Col xs={2}>
              <InputCheckbox label="Заблокирован" initialValue={this.state.blockUser.block} propName="block" onChange={this.onBlockChange} />
            </Col>
            <Col xs={6}>
              <InputText label="Причина" initialValue={this.state.blockUser.reason} propName="reason" onChange={this.onBlockChange} />
            </Col>
            <Col xs={3}>
              <Button color="primary" onClick={() => this.blockUser()}>Применить</Button>
            </Col>
          </Row>
        </Container>

        <h6 className="spaceTop">Роли пользователя</h6>
        <Container fluid>
          <Col xs={2}>
            <InputCheckbox label="Тренер" initialValue={this.state.userRoles.coach} propName="coach" onChange={this.onUserRoleChange} />
          </Col>
          <Col xs={2}>
            <InputCheckbox label="Администратор" initialValue={this.state.userRoles.admin} propName="admin" onChange={this.onUserRoleChange} />
          </Col>
          <Col xs={3}>
            <Button color="primary" onClick={() => this.applyUserRoles()}>Применить</Button>
          </Col>
        </Container>
      </>
    );
  }

}

export default WithRouter(UserAdministrationPanel)