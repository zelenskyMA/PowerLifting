import React from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';
import '../../../styling/Common.css';

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

function adminLink(userInfo) {
  if (!userInfo?.rolesInfo?.isAdmin) { return (<></>) }

  return (
    <NavItem style={{ marginRight: '20px' }}>
      <NavLink tag={Link} className="text-dark" to="/adminConsole">Администрирование</NavLink>
    </NavItem>
  );
}

function coachLink(userInfo) {
  if (!userInfo?.rolesInfo?.isCoach) { return (<></>) }

  return (
    <NavItem style={{ marginRight: '20px' }}>
      <NavLink tag={Link} className="text-dark" to="/coachConsole">Тренерская</NavLink>
    </NavItem>
  );
}