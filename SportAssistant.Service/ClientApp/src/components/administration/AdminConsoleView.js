import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import AppSettingsPanel from "./AppSettingsPanel";
import DictionariesPanel from "./DictionariesPanel";
import UserAdministrationPanel from "./UserAdministrationPanel";
import OrgListPanel from "../management/organization/OrgListPanel";

class AdminConsoleView extends Component {

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4 className="spaceBottom">{lngStr('appSetup.admin.console')}</h4>
        <TabControl data={[
          { id: 1, label: lngStr('appSetup.admin.users'), renderContent: () => this.usersContent() },
          { id: 2, label: lngStr('appSetup.admin.dictionaries'), renderContent: () => this.dictionaryContent() },
          { id: 3, label: lngStr('appSetup.admin.settings'), renderContent: () => this.settingsContent() },
          { id: 4, label: lngStr('appSetup.admin.orgs'), renderContent: () => this.orgsContent() },
        ]}
        />
      </>
    );
  }

  usersContent = () => { return (<UserAdministrationPanel />); }
  dictionaryContent = () => { return (<DictionariesPanel />); }
  settingsContent = () => { return (<AppSettingsPanel />); }
  orgsContent = () => { return (<OrgListPanel />); }
}

export default WithRouter(AdminConsoleView)