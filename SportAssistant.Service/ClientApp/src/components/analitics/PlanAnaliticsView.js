import React, { Component } from 'react';
import WithRouter from "../../common/extensions/WithRouter";
import PlanAnaliticsPanel from "./PlanAnaliticsPanel";

class PlanAnaliticsView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Аналитика</h3>
        <PlanAnaliticsPanel groupUserId="0" />
      </>
    );
  }
}

export default WithRouter(PlanAnaliticsView)