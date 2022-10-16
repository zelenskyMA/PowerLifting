import React, { Component } from 'react';
import { Button, Row, Col } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, DropdownControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';


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
    var cardData = await GetAsync(`/userInfo/getCard?userId=${this.props.params.id}`);
    var coachGroupsData = await GetAsync(`/trainingGroups/getList`);

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

  onGroupSelect = (id) => { this.setState({ error: '' }); this.setState({ selectedGroupId: id }); }

  changeGroup = async () => {
    try {
      var userGroup = { userId: this.props.params.id, groupId: this.state.selectedGroupId };
      await PostAsync(`/groupUser/update`, userGroup);
    }
    catch (error) { this.setState({ error: error.message }); }
  }

  removeUser = async () => {
    try {
      var userGroup = { userId: this.props.params.id, groupId: this.state.card?.groupInfo?.id };
      await PostAsync(`/groupUser/remove`, userGroup);
      this.props.navigate(`/coachConsole`);
    }
    catch (error) { this.setState({ error: error.message }); }
  }

  render() {
    if (this.state.loading) { return (<p><em>Загрузка...</em></p>); }

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop">
          <Col xs={2}>Рост: {this.state.card.baseInfo?.height ?? 0}</Col>
          <Col xs={2}>Возраст: {this.state.card.baseInfo?.age ?? 0}</Col>
          <Col xs={2}>Вес: {this.state.card.baseInfo?.weight ?? 0}</Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={2}>Рекорд в толчке: {this.state.pushAchivement?.result ?? 0}</Col>
          <Col xs={2}>Рекорд в рывке: {this.state.jerkAchivement?.result ?? 0}</Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={6}>
            <DropdownControl placeholder="Не задано" label="Укажите группу для перевода: "
              data={this.state.coachGroups} onChange={this.onGroupSelect} defaultValue={this.state.card.groupInfo.id} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={1} className="spaceRight">
            <Button color="primary" onClick={() => this.changeGroup()}>Перевести</Button>
          </Col>     
          <Col xs={1}>
            <Button color="primary" onClick={() => this.removeUser()}>Удалить</Button>
          </Col>
        </Row>
      </>
    );
  }
}

export default WithRouter(GroupUserCardPanel)