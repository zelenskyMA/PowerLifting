import React, { Component } from 'react';
import { connect } from "react-redux";
import { Link } from 'react-router-dom';
import {
    Collapse, DropdownItem, DropdownMenu, DropdownToggle, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink,
    UncontrolledDropdown
} from 'reactstrap';
import WithRouter from "../common/extensions/WithRouter";
import { GetToken, RemoveTokens } from '../common/TokenActions';
import { initApp } from "../stores/appStore/appActions";
import '../styling/Common.css';
import '../styling/NavMenu.css';

class NavMenu extends Component {
  constructor(props) {
    super(props);

    this.state = {
      collapsed: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    if (GetToken() != null) { await this.props.initApp(); }
  }

  toggleNavbar = () => { this.setState({ collapsed: !this.state.collapsed }); }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">Главная</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              {this.mainMenu()}
              {this.coachLink()}
              {this.adminLink()}
              {this.loginLink()}
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }

  mainMenu() {
    var legalName = this.props.userInfo?.legalName ?? '';
    if (legalName === '') { return (<></>); }

    return (
      <>
        <NavItem>
          <NavLink tag={Link} className="text-dark" to="/plansList">Планы</NavLink>
        </NavItem>

        <NavItem>
          <NavLink tag={Link} className="text-dark" to="/exercises">Упражнения</NavLink>
        </NavItem>

        <NavItem className="spaceRight">
          <NavLink tag={Link} className="text-dark" to="/planAnalitics">Аналитика</NavLink>
        </NavItem>
      </>
    );
  }

  loginLink() {
    var legalName = this.props.userInfo?.legalName ?? '';
    if (legalName === '') {
      return (
        <NavItem>
          <NavLink tag={Link} className="text-dark" to="/login">Вход</NavLink>
        </NavItem>
      );
    }

    return (
      <UncontrolledDropdown nav inNavbar>
        <DropdownToggle nav caret>{legalName}</DropdownToggle>
        <DropdownMenu end>
          <DropdownItem className="text-dark" tag={Link} to="/userCabinet" >Личный кабинет</DropdownItem>
          <DropdownItem className="text-dark" >Финансы</DropdownItem>
          <DropdownItem className="text-dark" onClick={() => RemoveTokens()} >Выход</DropdownItem>
        </DropdownMenu>
      </UncontrolledDropdown>
    );
  }

  adminLink() {
    if (!this.props.userInfo?.rolesInfo?.isAdmin) { return (<></>) }

    return (
      <NavItem style={{ marginRight: '20px' }}>
        <NavLink tag={Link} className="text-dark" to="/adminConsole">Администрирование</NavLink>
      </NavItem>
    );
  }

  coachLink() {
    if (!this.props.userInfo?.rolesInfo?.isCoach) { return (<></>) }

    return (
      <NavItem style={{ marginRight: '20px' }}>
        <NavLink tag={Link} className="text-dark" to="/coachConsole">Тренерская</NavLink>
      </NavItem>
    );
  }

}

const mapStateToProps = store => {
  return {
    userInfo: store.app.myInfo,
  }
}

const mapDispatchToProps = dispatch => {
  return {
    initApp: () => initApp(dispatch)
  }
}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(NavMenu))