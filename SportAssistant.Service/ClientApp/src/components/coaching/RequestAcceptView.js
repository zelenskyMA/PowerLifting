import React, { Component } from 'react';
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { DropdownControl, ErrorPanel, LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';


class RequestAcceptView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      request: Object,
      coachGroups: [],
      selectedGroupId: 0,
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var requestData = await GetAsync(`/trainingRequests/get?id=${this.props.params.requestId}`);
    var coachGroupsData = await GetAsync(`/trainingGroups/getList`);

    this.setState({ request: requestData, coachGroups: coachGroupsData, error: '', loading: false });
  }

  onGroupSelect = (id) => { this.setState({ error: '', selectedGroupId: id }); }

  rejectRequest = async () => {
    try {
      await PostAsync(`/trainingRequests/remove?userId=${this.state.request.userId}`);
      this.props.navigate(`/coachConsole`);
    }
    catch (error) { this.setState({ error: error.message }); }
  }

  acceptRequest = async () => {
    try {
      var userGroup = { userId: this.state.request.userId, groupId: this.state.selectedGroupId };
      await PostAsync(`/groupUser/update`, userGroup);
      this.props.navigate(`/coachConsole`);
    }
    catch (error) { this.setState({ error: error.message }); }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    return (
      <>
        <h5>{lngStr('coaching.request.fromUser') + ': ' + this.state.request.userName}</h5>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={2}>{lngStr('appSetup.user.height') + ': ' + this.state.request.userHeight}</Col>
          <Col xs={2}>{lngStr('appSetup.user.age') + ': ' + this.state.request.userAge}</Col>
          <Col xs={2}>{lngStr('appSetup.user.weight') + ': ' + this.state.request.userWeight}</Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={6}>
            <DropdownControl placeholder={lngStr('general.common.notSet')} label={lngStr('coaching.groups.setGroupForSportsman') + ': '} noData={lngStr('coaching.groups.createGroupForSportsman')}
              data={this.state.coachGroups} onChange={this.onGroupSelect} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={1} className="spaceRight">
            <Button color="primary" onClick={() => this.rejectRequest()}>{lngStr('general.actions.reject')}</Button>
          </Col>
          <Col xs={1}>
            <Button color="primary" onClick={() => this.acceptRequest()}>{lngStr('general.actions.accept')}</Button>
          </Col>
        </Row>
      </>
    );
  }
}

export default WithRouter(RequestAcceptView)