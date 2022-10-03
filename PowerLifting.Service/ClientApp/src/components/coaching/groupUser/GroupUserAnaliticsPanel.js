import React, { Component } from 'react';
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import PlanAnaliticsView from "../../analitics/PlanAnaliticsView";

class GroupUserAnaliticsPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userId: 0
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    this.setState({ userId: this.props.params.id });
  }

  render() {
    return (
      <>
        <PlanAnaliticsView userId={this.state.userId} />
      </>
    );
  }
}

export default WithRouter(GroupUserAnaliticsPanel)