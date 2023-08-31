import React from 'react';
import { Link } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import { NavItem, NavLink } from 'reactstrap';
import '../../../styling/Common.css';
import '../../../styling/NavMenu.css';

export function OrgOwnerTopMenu({ userInfo }) {
  const { t } = useTranslation();

  return (
    <>
      {mainMenu(userInfo, t)}
    </>);
}

function mainMenu(userInfo, lngStr) {
  var legalName = userInfo?.legalName ?? '';
  if (legalName === '' || !userInfo?.rolesInfo?.isOrgOwner) { return (<></>); }

  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/manager/list">{lngStr('management.forManager')}</NavLink>
      </NavItem>

      <NavItem>
        <NavLink tag={Link} className="menu-item" to="/finance">{lngStr('management.finance')}</NavLink>
      </NavItem>
    </>
  );
}