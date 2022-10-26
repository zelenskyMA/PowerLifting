import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import AppSettingsPanel from "./AppSettingsPanel";
import DictionariesPanel from "./DictionariesPanel";
import UserAdministrationPanel from "./UserAdministrationPanel";

class AdminConsoleView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h4 className="spaceBottom">Административная консоль</h4>
        <TabControl data={[
          { id: 1, label: 'Ползователи', renderContent: () => this.usersContent() },
          { id: 2, label: 'Справочники', renderContent: () => this.dictionaryContent() },
          { id: 3, label: 'Настройки', renderContent: () => this.settingsContent() },
        ]}
        />
      </>
    );
  }

  usersContent = () => { return (<UserAdministrationPanel />); }
  dictionaryContent = () => { return (<DictionariesPanel />); }
  settingsContent = () => { return (<AppSettingsPanel />); }
}

export default WithRouter(AdminConsoleView)