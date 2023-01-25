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
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4 className="spaceBottom">{lngStr('admin.console')}</h4>
        <TabControl data={[
          { id: 1, label: lngStr('admin.users'), renderContent: () => this.usersContent() },
          { id: 2, label: lngStr('admin.dictionaries'), renderContent: () => this.dictionaryContent() },
          { id: 3, label: lngStr('admin.settings'), renderContent: () => this.settingsContent() },
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