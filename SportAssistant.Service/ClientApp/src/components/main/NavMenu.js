import React, { Component } from 'react';
import { connect } from "react-redux";
import { Link } from 'react-router-dom';
import {
  Collapse, DropdownItem, DropdownMenu, DropdownToggle, Navbar, NavbarBrand, NavbarToggler, UncontrolledDropdown, NavItem, NavLink
} from 'reactstrap';
import WithRouter from "../../common/extensions/WithRouter";
import { RemoveTokens } from '../../common/TokenActions';
import '../../styling/Common.css';
import '../../styling/NavMenu.css';
import { CoachTopMenu } from "./menu/CoachTopMenu";
import { CommonTopMenu } from "./menu/CommonTopMenu";

const mapStateToProps = store => {
  return {
    userInfo: store.app.myInfo,
  }
}

class NavMenu extends Component {
  constructor(props) {
    super(props);

    this.state = {
      collapsed: true
    };
  }

  toggleNavbar = () => { this.setState({ collapsed: !this.state.collapsed }); }

  render() {
    const lngStr = this.props.lngStr;
    var legalName = this.props.userInfo?.legalName ?? '';

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white box-shadow mb-3 navbar-dark" container light>
          {legalName &&
            <>
              <NavbarBrand className="menu-first-page-item" tag={Link} to="/">{lngStr('general.common.main')}</NavbarBrand>

            <NavbarBrand className="help-menu-item" tag={Link} to="/help/0">{lngStr('general.common.help')}</NavbarBrand>

            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            </>
          }
          <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
            <ul className="navbar-nav flex-grow">
              {this.props.userInfo?.coachOnly ?
                <CoachTopMenu userInfo={this.props.userInfo} /> :
                <CommonTopMenu userInfo={this.props.userInfo} />}
              {this.loginLink(lngStr)}
            </ul>
          </Collapse>
        </Navbar>
      </header>
    );
  }

  loginLink(lngStr) {
    var legalName = this.props.userInfo?.legalName ?? '';
    if (legalName === '') { return (<></>); }

    return (
      <UncontrolledDropdown nav inNavbar>
        <DropdownToggle className="menu-item" nav caret>{legalName}</DropdownToggle>
        <DropdownMenu end>
          <DropdownItem className="text-dark" tag={Link} to="/userCabinet" >{lngStr('appSetup.user.cabinet')}</DropdownItem>
          <DropdownItem className="text-dark" onClick={() => RemoveTokens()} >{lngStr('general.common.exit')}</DropdownItem>
        </DropdownMenu>
      </UncontrolledDropdown>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(NavMenu))