import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { NavItem, NavLink } from 'reactstrap';
import '../../../styling/Common.css';
import '../../../styling/NavMenu.css';

export function CoachTopMenu({ userInfo }) {
  const { t } = useTranslation();

  return (<>{mainMenu(userInfo, t)}</>);
}

function mainMenu(userInfo, lngStr) {
  var legalName = userInfo?.legalName ?? '';
  if (legalName === '' || !userInfo?.rolesInfo?.isCoach) { return (<></>); }

  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/groupConsole">{lngStr('coaching.groups.header')}</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/exercises">{lngStr('training.exercise.exercises')}</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/templateSetList">{lngStr('training.cycle.multi')}</NavLink>
      </NavItem>

      <NavItem className="spaceRight">
        <NavLink tag={Link} className="menu-item" to="/requestConsole">{lngStr('coaching.request.header')}</NavLink>
      </NavItem>
    </>
  );
}