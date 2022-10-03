import React, { Component } from 'react';
import { connect } from "react-redux";
import { setGroupUserId } from "../../stores/coachingStore/coachActions";
import PlanAnaliticsPanel from "./PlanAnaliticsPanel";
import WithRouter from "../../common/extensions/WithRouter";

const mapDispatchToProps = dispatch => {
  return {
    setGroupUserId: (userId) => setGroupUserId(userId, dispatch)
  }
}

class PlanAnaliticsView extends Component {
  constructor(props) {
    super(props);

    this.props.setGroupUserId(0);
  }

  render() {
    return (
      <>
        <h3 className="spaceBottom">Аналитика</h3>
        <PlanAnaliticsPanel />
      </>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(PlanAnaliticsView))