import React, { Component } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../styling/NavMenu.css';

export class NavMenu extends Component {
  constructor(props) {
    super(props);

    this.state = {
      collapsed: true
    };
  }

/*
  componentDidMount() { this.getuserInfo(); }

  getuserInfo = async () => {
    var plan = await GetAsync("/userInfo/get");
    this.setState({ plannedDays: plan.trainingDays, typeCounters: plan.typeCountersSum });
  }
*/

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
              <NavItem>
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
    return (
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/login">Вход</NavLink>
      </NavItem>
    );
  }

}