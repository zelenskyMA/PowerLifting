import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import PlansListPanel from "../../trainingPlan/view/PlansListPanel";

class GroupUserAnaliticsPanel extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <PlansListPanel />
    );
  }
}

export default WithRouter(GroupUserAnaliticsPanel)