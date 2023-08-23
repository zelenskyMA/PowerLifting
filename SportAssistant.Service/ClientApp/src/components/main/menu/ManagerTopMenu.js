import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { NavItem, NavLink } from 'reactstrap';
import '../../../styling/Common.css';
import '../../../styling/NavMenu.css';

export function ManagerTopMenu({ userInfo }) {
  const { t } = useTranslation();

  return (
    <>
      {mainMenu(userInfo, t)}
    </>);
}

function mainMenu(userInfo, lngStr) {
  var legalName = userInfo?.legalName ?? '';
  if (legalName === '' || !userInfo?.rolesInfo?.isManager) { return (<></>); }

  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/assignedCoaches/list">{lngStr('management.assignedCoach.coachManagement')}</NavLink>
      </NavItem>
    </>
  );
}