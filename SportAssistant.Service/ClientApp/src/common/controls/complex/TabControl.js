import React, { useState } from 'react';
import { Nav, NavItem, NavLink, TabContent, TabPane } from "reactstrap";
import classnames from 'classnames';

export function TabControl({ data }) {
  const [activeTabId, setActiveTab] = useState(1);

  if (data?.length === 0) { return (<></>); }

  return (
    <>
      <Nav tabs>
        {data.map((item) => {
          return (
            <NavItem key={'tabHeader_' + item.id} role="button" onClick={() => setActiveTab(item.id)}>
              <NavLink className={classnames({ active: activeTabId === item.id })} >
                {item.label}
              </NavLink>
            </NavItem>
          );
        })}
      </Nav>
      <TabContent activeTab={activeTabId.toString()}>
        {data.map((item) => {         
          return (
            <TabPane key={'tabPane_' + item.id} tabId={item.id.toString()}>
              {item.renderContent()}
            </TabPane>
          );
        })}
      </TabContent>
    </>
  );
}