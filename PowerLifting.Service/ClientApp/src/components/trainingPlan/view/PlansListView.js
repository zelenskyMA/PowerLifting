import React, { Component } from 'react';
import { connect } from "react-redux";
import WithRouter from "../../../common/extensions/WithRouter";
import { setGroupUserId } from "../../../stores/coachingStore/coachActions";
import PlansListPanel from "./PlansListPanel";

const mapDispatchToProps = dispatch => {
  return {
    setGroupUserId: (userId) => setGroupUserId(userId, dispatch)
  }
}

class PlansListView extends Component {
  constructor(props) {
    super(props);

    this.props.setGroupUserId(0);    
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Тренировочные планы</h3>
        <PlansListPanel />
      </>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(PlansListView));