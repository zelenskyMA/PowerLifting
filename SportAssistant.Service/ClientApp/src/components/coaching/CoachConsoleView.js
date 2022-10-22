import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import GroupConsolePanel from "./GroupConsolePanel";
import RequestConsolePanel from "./RequestConsolePanel";
import TemplateSetListView from "../trainingTemplate/view/TemplateSetListView";

class CoachConsoleView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h5 className="spaceBottom">Тренерский кабинет</h5>
        <TabControl data={[
          { id: 1, label: 'Группы', renderContent: () => this.groupsContent() },
          { id: 2, label: 'Тренировочные циклы', renderContent: () => this.templateSetsContent() },
          { id: 3, label: 'Заявки', renderContent: () => this.requestsContent() },
        ]}
        />
      </>
    );
  }

  groupsContent = () => { return (<GroupConsolePanel />); }
  templateSetsContent = () => { return (<TemplateSetListView />); }
  requestsContent = () => { return (<RequestConsolePanel />); }
}

export default WithRouter(CoachConsoleView)