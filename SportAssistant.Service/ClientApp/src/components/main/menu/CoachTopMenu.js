import React from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';
import '../../../styling/Common.css';
import '../../../styling/NavMenu.css';

export function CoachTopMenu({ userInfo }) {
  return (<>
    {mainMenu(userInfo)}
  </>);
}

function mainMenu(userInfo) {
  var legalName = userInfo?.legalName ?? '';
  if (legalName === '' || !userInfo?.rolesInfo?.isCoach) { return (<></>); }

  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/groupConsole">Группы</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/exercises">Упражнения</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/templateSetList">Тренировочные циклы</NavLink>
      </NavItem>

      <NavItem className="spaceRight">
        <NavLink tag={Link} className="menu-item" to="/requestConsole">Заявки</NavLink>
      </NavItem>
    </>
  );
}