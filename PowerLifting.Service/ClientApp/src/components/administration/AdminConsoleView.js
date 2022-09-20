import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import UserAdministrationPanel from "./UserAdministrationPanel";
import DictionariesPanel from "./DictionariesPanel";
import '../../styling/Common.css';

class AdminConsoleView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Административная консоль</h3>
        <TabControl data={[
          { id: 1, label: 'Ползователи', renderContent: () => this.usersContent() },
          { id: 2, label: 'Справочники', renderContent: () => this.dictionaryContent() }
        ]}
        />
      </>
    );
  }

  usersContent = () => { return (<UserAdministrationPanel />); }
  dictionaryContent = () => { return (<DictionariesPanel />); }
}

export default WithRouter(AdminConsoleView)