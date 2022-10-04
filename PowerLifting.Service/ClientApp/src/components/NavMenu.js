import React, { Component } from 'react';
import {
  Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink,
  UncontrolledDropdown, DropdownToggle, DropdownMenu, DropdownItem
} from 'reactstrap';
import { Link } from 'react-router-dom';
import { GetAsync } from "../common/ApiActions";
import { GetToken, RemoveTokens } from '../common/TokenActions';
import '../styling/NavMenu.css';

export class NavMenu extends Component {
  constructor(props) {
    super(props);

    this.state = {
      collapsed: true,
      userInfo: Object
    };
  }

  componentDidMount() { this.getUserInfo(); }

  getUserInfo = async () => {
    if (GetToken() == null) {
      return;
    }

    var info = await GetAsync("/userInfo/get");
    this.setState({ userInfo: info });
  }

  toggleNavbar = () => { this.setState({ collapsed: !this.state.collapsed }); }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">Спортивный ассистент</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">

              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/planAnalitics">Аналитика</NavLink>
              </NavItem>

              <NavItem style={{ marginRight: '20px' }}>
                <NavLink tag={Link} className="text-dark" to="/plansList">Планы</NavLink>
              </NavItem>

              {this.coachGroupsLink()}

              {this.adminLink()}

              {this.loginLink()}
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }

  loginLink() {
    var legalName = this.state.userInfo?.legalName ?? '';
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
    if (!this.state.userInfo?.rolesInfo?.isAdmin) { return (<></>) }

    return (
      <NavItem style={{ marginRight: '20px' }}>
        <NavLink tag={Link} className="text-dark" to="/adminConsole">Администрирование</NavLink>
      </NavItem>
    );
  }

  coachGroupsLink() {
    if (!this.state.userInfo?.rolesInfo?.isCoach) { return (<></>) }

    return (
      <NavItem style={{ marginRight: '20px' }}>
        <NavLink tag={Link} className="text-dark" to="/coachConsole">Тренерская</NavLink>
      </NavItem>
    );
  }

}