import React, { Component } from 'react';
import { Button, Col, Container, Row } from "reactstrap";
import { PostAsync } from "../../common/ApiActions";
import { ErrorPanel, InfoPanel, InputCheckbox, InputText, UserSearchControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import { UserCardModal } from "../userData/UserCardModal";

class UserAdministrationPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      blockUser: { block: false, reason: '' },
      userRoles: { admin: false, coach: false, manager: false, orgOwner: false },

      userCard: Object,
      userCardView: false,
      error: '',
      success: ''
    };
  }

  onBlockChange = (propName, value) => { this.setState(prevState => ({ blockUser: { ...prevState.blockUser, [propName]: value } })); }
  onUserRoleChange = (propName, value) => { this.setState(prevState => ({ userRoles: { ...prevState.userRoles, [propName]: value } })); }

  onUserCardView = (value) => { this.setState({ userCardView: value }); }

  handleSearchResult = (userCard, errorText) => {
    if (errorText) {
      this.setState({ error: errorText });
      return;
    }

    var blockReason = userCard?.blockReason?.reason == null ? '' : userCard?.blockReason?.reason;
    var roles = userCard?.baseInfo?.rolesInfo;

    this.setState({
      userCard: userCard,
      userRoles: { admin: roles?.isAdmin, coach: roles?.isCoach, manager: roles?.isManager, orgOwner: roles?.isOrgOwner },
      blockUser: { block: blockReason !== '', reason: blockReason },
      error: '',
      success: ''
    });
  }

  blockUser = async (lngStr) => {
    try {
      var blockInfo = {
        userId: this.state.userCard?.userId,
        status: this.state.blockUser.block,
        reason: this.state.blockUser.reason
      }

      await PostAsync('/administration/applyBlock', blockInfo);

      this.setState({ success: lngStr('appSetup.admin.blockChanged'), error: '' });
    }
    catch (error) {
      this.setState({ error: error.message, success: '' });
    }
  }

  applyUserRoles = async (lngStr) => {
    try {
      var roleInfo = {
        userId: this.state.userCard?.userId,
        isAdmin: this.state.userRoles.admin,
        isCoach: this.state.userRoles.coach,
        isManager: this.state.userRoles.manager,
        isOrgOwner: this.state.userRoles.orgOwner
      }

      await PostAsync('/administration/applyRoles', roleInfo);
      this.setState({ success: lngStr('appSetup.admin.rolesChanged'), error: '' });
    }
    catch (error) {
      this.setState({ error: error.message, success: '' });
    }
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <p className="spaceTop">{lngStr('appSetup.admin.userSearch')}</p>
        <ErrorPanel errorMessage={this.state.error} />
        <InfoPanel infoMessage={this.state.success} />

        <UserSearchControl handleSearchResult={this.handleSearchResult} />
        <hr style={{ width: '80%', paddingTop: "2px" }} />

        {this.loadedUserInfo(lngStr)}
      </>
    );
  }

  loadedUserInfo = (lngStr) => {
    if (this.state.userCard?.userId == null) { return (<></>); }

    return (
      <>
        <h6 className="spaceTop"> {lngStr('appSetup.admin.userFound')}
          <span id="legalName" role="Button" style={{ marginLeft: '10px', color: 'blue', cursor: 'zoom-in' }}
            onMouseEnter={() => this.onUserCardView(true)} onMouseLeave={() => this.onUserCardView(false)}>
            {this.state.userCard?.baseInfo?.legalName}
          </span>
        </h6>
        <UserCardModal userInfo={this.state.userCard?.baseInfo} targetId="legalName" isOpen={this.state.userCardView} />

        <Container fluid>
          <Row>
            <Col xs={3}>
              {lngStr('general.id') + ': ' + this.state.userCard?.userId}
            </Col>
            <Col xs={3}>
              {lngStr('general.auth.login') + ': ' + this.state.userCard?.login}
            </Col>
          </Row>
        </Container>

        <h6 className="spaceTop">{lngStr('general.actions.block')}</h6>
        <Container fluid>
          <Row>
            <Col xs={2}>
              <InputCheckbox label={lngStr('appSetup.admin.blocked')} initialValue={this.state.blockUser.block} propName="block" onChange={this.onBlockChange} />
            </Col>
            <Col xs={6}>
              <InputText label={lngStr('appSetup.admin.reason')} initialValue={this.state.blockUser.reason} propName="reason" onChange={this.onBlockChange} />
            </Col>
            <Col xs={3}>
              <Button color="primary" onClick={() => this.blockUser(lngStr)}>{lngStr('general.actions.confirm')}</Button>
            </Col>
          </Row>
        </Container>

        <h6 className="spaceTop">{lngStr('appSetup.admin.userRoles')}</h6>
        <Container fluid>
          <Row>
            <Col xs={2}>
              <InputCheckbox label={lngStr('coaching.trainer')} initialValue={this.state.userRoles.coach} propName="coach" onChange={this.onUserRoleChange} />
            </Col>
            <Col xs={2}>
              <InputCheckbox label={lngStr('appSetup.admin.adminRole')} initialValue={this.state.userRoles.admin} propName="admin" onChange={this.onUserRoleChange} />
            </Col>
          </Row>
          <Row>
            <Col xs={2}>
              <InputCheckbox disabled label={lngStr('management.manager')} initialValue={this.state.userRoles.manager} propName="manager" onChange={this.onUserRoleChange} />
            </Col>
            <Col xs={2}>
              <InputCheckbox disabled label={lngStr('management.org.owner')} initialValue={this.state.userRoles.orgOwner} propName="orgOwner" onChange={this.onUserRoleChange} />
            </Col>
          </Row>

          <Col xs={3} className="spaceTopXs">
            <Button color="primary" onClick={() => this.applyUserRoles(lngStr)}>{lngStr('general.actions.confirm')}</Button>
          </Col>
        </Container>
      </>
    );
  }

}

export default WithRouter(UserAdministrationPanel)