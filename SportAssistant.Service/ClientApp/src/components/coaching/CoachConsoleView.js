import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import GroupConsolePanel from "./GroupConsolePanel";
import RequestConsolePanel from "./RequestConsolePanel";

class CoachConsoleView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Тренерский кабинет</h3>
        <TabControl data={[
          { id: 1, label: 'Группы', renderContent: () => this.groupsContent() },
          { id: 2, label: 'Заявки', renderContent: () => this.requestsContent() },
        ]}
        />
      </>
    );
  }

  groupsContent = () => { return (<GroupConsolePanel />); }
  requestsContent = () => { return (<RequestConsolePanel />); }
}

export default WithRouter(CoachConsoleView)