import React, { Component } from 'react';
import { TabControl } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import TemplateSetListView from "../trainingTemplate/view/TemplateSetListView";
import GroupConsoleView from "./GroupConsoleView";
import RequestConsoleView from "./RequestConsoleView";

class CoachConsoleView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <h5 className="spaceBottom">{lngStr('coaching.coachCabinet')}</h5>
        <TabControl data={[
          { id: 1, label: lngStr('coaching.groups.header'), renderContent: () => this.groupsContent() },
          { id: 2, label: lngStr('training.cycle.multi'), renderContent: () => this.templateSetsContent() },
          { id: 3, label: lngStr('coaching.request.header'), renderContent: () => this.requestsContent() },
        ]}
        />
      </>
    );
  }

  groupsContent = () => { return (<GroupConsoleView />); }
  templateSetsContent = () => { return (<TemplateSetListView />); }
  requestsContent = () => { return (<RequestConsoleView />); }
}

export default WithRouter(CoachConsoleView)