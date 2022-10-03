import React, { Component } from 'react';
import { TabControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { GetAsync } from "../../../common/ApiActions";
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
    return (
      <>
        <h3>{this.state.userName}</h3>
        <TabControl data={[
          { id: 1, label: 'Карточка', renderContent: () => this.userCardContent() },
          { id: 2, label: 'Планы', renderContent: () => this.plansContent() },
          { id: 3, label: 'Аналитика', renderContent: () => this.analiticsContent() }
        ]}
        />
      </>
    );
  }

  userCardContent = () => { return (<GroupUserCardPanel />); }
  plansContent = () => { return (<GroupUserPlansPanel />); }
  analiticsContent = () => { return (<GroupUserAnaliticsPanel />); }
}

export default WithRouter(GroupUserView)