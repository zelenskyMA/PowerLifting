import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import PlanAnaliticsPanel from "../../analitics/PlanAnaliticsPanel";

class GroupUserAnaliticsPanel extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <PlanAnaliticsPanel />
      </>
    );
  }
}

export default WithRouter(GroupUserAnaliticsPanel)