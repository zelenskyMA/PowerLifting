import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import GroupConsolePanel from "./GroupConsolePanel";
import RequestConsolePanel from "./RequestConsolePanel";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class CoachConsoleView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Тренерский кабинет</h3>
        <TabControl data={[
          { id: 1, label: 'Заявки', renderContent: () => this.requestsContent() },
          { id: 2, label: 'Группы', renderContent: () => this.groupsContent() }
        ]}
        />
      </>
    );
  }

  groupsContent = () => { return (<GroupConsolePanel />); }
  requestsContent = () => { return (<RequestConsolePanel />); }
}

export default WithRouter(CoachConsoleView)