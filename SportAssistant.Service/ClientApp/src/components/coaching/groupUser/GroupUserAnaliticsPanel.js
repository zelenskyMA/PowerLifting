import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import PlanAnaliticsPanel from "../../analitics/PlanAnaliticsPanel";

class GroupUserAnaliticsPanel extends Component {  
  render() {
    return (
      <>
        <PlanAnaliticsPanel groupUserId={this.props.groupUserId} />
      </>
    );
  }
}

export default WithRouter(GroupUserAnaliticsPanel)