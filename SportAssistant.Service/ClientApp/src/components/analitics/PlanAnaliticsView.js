import React, { Component } from 'react';
import WithRouter from "../../common/extensions/WithRouter";
import PlanAnaliticsPanel from "./PlanAnaliticsPanel";

class PlanAnaliticsView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <h3 className="spaceBottom">{lngStr('analitics.header')}</h3>
        <PlanAnaliticsPanel groupUserId="0" />
      </>
    );
  }
}

export default WithRouter(PlanAnaliticsView)