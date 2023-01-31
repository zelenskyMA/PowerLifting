import React, { Component } from 'react';
import { GetAsync } from "../../../common/ApiActions";
import { TabControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import GroupUserAnaliticsPanel from "./GroupUserAnaliticsPanel";
import GroupUserCardPanel from "./GroupUserCardPanel";
import GroupUserPlansPanel from "./GroupUserPlansPanel";

class GroupUserView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userName: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var cardData = await GetAsync(`/userInfo/getCard?userId=${this.props.params.id}`);
    this.setState({ userName: cardData.userName });
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <h4>{this.state.userName}</h4>
        <TabControl data={[
          { id: 1, label: lngStr('training.plan.multi'), renderContent: () => this.plansContent() },
          { id: 2, label: lngStr('appSetup.user.userCard'), renderContent: () => this.userCardContent() },
          { id: 3, label: lngStr('analitics.header'), renderContent: () => this.analiticsContent() }
        ]}
        />
      </>
    );
  }

  userCardContent = () => { return (<GroupUserCardPanel />); }
  plansContent = () => { return (<GroupUserPlansPanel groupUserId={this.props.params.id} />); }
  analiticsContent = () => { return (<GroupUserAnaliticsPanel groupUserId={this.props.params.id} />); }
}

export default WithRouter(GroupUserView);