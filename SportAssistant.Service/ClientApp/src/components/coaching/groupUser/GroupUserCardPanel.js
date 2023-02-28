import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { DropdownControl, ErrorPanel, LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import '../../../styling/Common.css';

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class GroupUserCardPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      card: Object,
      coachGroups: [],
      selectedGroupId: 0,
      pushAchivement: Object, // толчок, id = 1
      jerkAchivement: Object, // рывок, id = 2
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    try {
      const [cardData, coachGroupsData] = await Promise.all([
        GetAsync(`/userInfo/getCard/${this.props.params.id}`),
        GetAsync(`/trainingGroups/getList`)
      ]);

      var push = cardData.achivements.find(t => t.exerciseTypeId === 1);
      var jerk = cardData.achivements.find(t => t.exerciseTypeId === 2);

      this.setState({
        card: cardData,
        coachGroups: coachGroupsData,
        pushAchivement: push,
        jerkAchivement: jerk,
        error: '',
        loading: false
      });
    }
    catch (error) { this.setState({ error: error.message, loading: false }); }
  }

  onGroupSelect = (id) => { this.setState({ error: '', selectedGroupId: id }); }

  changeGroup = async () => {
    try {
      var userGroup = { userId: this.props.params.id, groupId: this.state.selectedGroupId };
      await PostAsync(`/groupUser/assign`, userGroup);
    }
    catch (error) { this.setState({ error: error.message }); }
  }

  confirmRemoveUser = async () => {
    try {
      var userGroup = { userId: this.props.params.id, groupId: this.state.card?.groupInfo?.id };
      await PostAsync(`/groupUser/remove`, userGroup);
      this.props.navigate(`/coachConsole`);
    }
    catch (error) { this.setState({ error: error.message }); }
  }

  removeUser = async (lngStr) => {
    var modalInfo = {
      isVisible: true,
      headerText: lngStr('appSetup.modal.confirm'),
      buttons: [{ name: lngStr('general.actions.confirm'), onClick: this.confirmRemoveUser, color: "success" }],
      body: () => { return (<p>{lngStr('coaching.groups.confirmSportsmenRemoval')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={2}>{lngStr('appSetup.user.height') + ': ' + (this.state.card.baseInfo?.height ?? 0)}</Col>
          <Col xs={2}>{lngStr('appSetup.user.age') + ': ' + (this.state.card.baseInfo?.age ?? 0)}</Col>
          <Col xs={2}>{lngStr('appSetup.user.weight') + ': ' + (this.state.card.baseInfo?.weight ?? 0)}</Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={2}>{lngStr('appSetup.user.pushAchivement') + ': ' + (this.state.pushAchivement?.result ?? 0)}</Col>
          <Col xs={2}>{lngStr('appSetup.user.jerkAchivement') + ': ' + (this.state.jerkAchivement?.result ?? 0)}</Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={6}>
            <DropdownControl placeholder={lngStr('general.common.notSet')} label={lngStr('coaching.groups.groupForTransfer') + ': '}
              data={this.state.coachGroups} onChange={this.onGroupSelect} defaultValue={this.state.card.groupInfo.id} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={1} className="spaceRight">
            <Button color="primary" onClick={() => this.changeGroup()}>{lngStr('general.actions.transfer')}</Button>
          </Col>
          <Col xs={1}>
            <Button color="primary" onClick={() => this.removeUser(lngStr)}>{lngStr('general.actions.delete')}</Button>
          </Col>
        </Row>
      </>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(GroupUserCardPanel))