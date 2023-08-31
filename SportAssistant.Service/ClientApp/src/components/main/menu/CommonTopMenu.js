import React from 'react';
import { Link } from 'react-router-dom';
import { NavItem, NavLink } from 'reactstrap';
import { useTranslation } from 'react-i18next';
import '../../../styling/Common.css';
import '../../../styling/NavMenu.css';

export function CommonTopMenu({ userInfo }) {
  const { t } = useTranslation();

  return (
    <>
      {mainMenu(userInfo, t)}
      {coachLink(userInfo, t)}
      {adminLink(userInfo, t)}
    </>);
}

function mainMenu(userInfo, lngStr) {
  var legalName = userInfo?.legalName ?? '';
  if (legalName === '') { return (<></>); }

  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/plansList">{lngStr('training.plan.multi')}</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/exercises">{lngStr('training.exercise.exercises')}</NavLink>
      </NavItem>

      <NavItem className="spaceRight">
        <NavLink tag={Link} className="menu-item" to="/planAnalitics">{lngStr('analitics.header')}</NavLink>
      </NavItem>
    </>
  );
}

function adminLink(userInfo, lngStr) {
  if (!userInfo?.rolesInfo?.isAdmin) { return (<></>) }

  return (
    <NavItem className="menu-spaceRight">
      <NavLink tag={Link} className="menu-item" to="/adminConsole">{lngStr('appSetup.admin.administration')}</NavLink>
    </NavItem>
  );
}

function coachLink(userInfo, lngStr) {
  if (!userInfo?.rolesInfo?.isCoach) { return (<></>) }

  return (
    <NavItem className="menu-spaceRight">
      <NavLink tag={Link} className="menu-item" to="/coachConsole">{lngStr('coaching.coachRoom')}</NavLink>
    </NavItem>
  );
}