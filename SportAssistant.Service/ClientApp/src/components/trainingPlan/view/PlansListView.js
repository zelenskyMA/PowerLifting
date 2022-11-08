import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import PlansListPanel from "./PlansListPanel";

class PlansListView extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <>
        <h4 className="spaceBottom">Тренировочные планы</h4>
        <PlansListPanel groupUserId="0" />
      </>
    );
  }
}

export default WithRouter(PlansListView);