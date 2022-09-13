import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { GetAsync } from "../common/ApiActions";
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
    var info = await GetAsync("/userInfo/get");
    this.setState({ userInfo: info });
  }

  toggleNavbar = () => { this.setState({ collapsed: !this.state.collapsed }); }

  render() {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
          <NavbarBrand tag={Link} to="/">Помощник атлета</NavbarBrand>
          <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">Главная</NavLink>
              </NavItem>
              <NavItem style={{ marginRight: '20px' }}>
                <NavLink tag={Link} className="text-dark" to="/createPlan">Новый план</NavLink>
              </NavItem>
              {this.loginPanel()}
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }

  loginPanel() {
    var legalName = this.state.userInfo?.legalName ?? '';
    if (legalName === '') {
      return (
        <NavItem>
          <NavLink tag={Link} className="text-dark" to="/login">Вход</NavLink>
        </NavItem>
      );
    }

    return (
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/userCabinet" title="Личный кабинет">{legalName}</NavLink>
      </NavItem>
    );
  }

}