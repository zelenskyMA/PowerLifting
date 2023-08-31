import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import PlansListPanel from "./PlansListPanel";

class PlansListView extends Component {
  
  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4 className="spaceBottom">{lngStr('training.plan.trainingPlans')}</h4>
        <PlansListPanel groupUserId="0" />
      </>
    );
  }
}

export default WithRouter(PlansListView);