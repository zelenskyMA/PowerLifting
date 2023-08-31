import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import PlansListPanel from "../../trainingPlan/view/PlansListPanel";

class GroupUserPlansPanel extends Component {
  render() {
    return (
      <PlansListPanel groupUserId={this.props.groupUserId} />
    );
  }
}

export default WithRouter(GroupUserPlansPanel)