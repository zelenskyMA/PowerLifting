import React from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';
import '../../../styling/Common.css';
import '../../../styling/NavMenu.css';

export function CommonTopMenu({ userInfo }) {
  return (<>
    {mainMenu(userInfo)}
    {coachLink(userInfo)}
    {adminLink(userInfo)}
  </>);
}

function mainMenu(userInfo) {
  var legalName = userInfo?.legalName ?? '';
  if (legalName === '') { return (<></>); }

  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/plansList">Планы</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/exercises">Упражнения</NavLink>
      </NavItem>

      <NavItem className="spaceRight">
        <NavLink tag={Link} className="menu-item" to="/planAnalitics">Аналитика</NavLink>
      </NavItem>
    </>
  );
}

function adminLink(userInfo) {
  if (!userInfo?.rolesInfo?.isAdmin) { return (<></>) }

  return (
    <NavItem className="menu-spaceRight">
      <NavLink tag={Link} className="menu-item" to="/adminConsole">Администрирование</NavLink>
    </NavItem>
  );
}

function coachLink(userInfo) {
  if (!userInfo?.rolesInfo?.isCoach) { return (<></>) }

  return (
    <NavItem className="menu-spaceRight">
      <NavLink tag={Link} className="menu-item" to="/coachConsole">Тренерская</NavLink>
    </NavItem>
  );
}