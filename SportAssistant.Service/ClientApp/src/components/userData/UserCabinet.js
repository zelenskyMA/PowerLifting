import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Label, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { InputNumber, InputCheckbox, InputText, LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import { changeModalVisibility, updateUserInfo } from "../../stores/appStore/appActions";
import '../../styling/Common.css';

const mapDispatchToProps = dispatch => {
  return {
    updateUserInfo: (userInfo) => updateUserInfo(userInfo, dispatch),
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

class UserCabinet extends Component {
  constructor(props) {
    super(props);

    this.state = {
      userInfo: Object,
      trainingRequest: Object,
      pushAchivement: Object, // толчок, id = 1
      jerkAchivement: Object, // рывок, id = 2
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    const [info, achivementsData, trainingRequestData] = await Promise.all([
      GetAsync("/userInfo/get"),
      GetAsync("/userAchivement/get"),
      GetAsync("/trainingRequests/getMyRequest")
    ]);

    var push = achivementsData.find(t => t.exerciseTypeId === 1);
    var jerk = achivementsData.find(t => t.exerciseTypeId === 2);

    this.setState({ userInfo: info, trainingRequest: trainingRequestData, pushAchivement: push, jerkAchivement: jerk, loading: false });
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ userInfo: { ...prevState.userInfo, [propName]: value } })); }
  onPushChange = (propName, value) => { this.setState(prevState => ({ pushAchivement: { ...prevState.pushAchivement, [propName]: value } })); }
  onJerkChange = (propName, value) => { this.setState(prevState => ({ jerkAchivement: { ...prevState.jerkAchivement, [propName]: value } })); }

  createRequest = () => { this.props.navigate("/coachSelection"); }
  cancelRequest = async () => {
    await PostAsync(`/trainingRequests/remove`);
    var trainingRequestData = await GetAsync("/trainingRequests/getMyRequest");
    this.setState({ trainingRequest: trainingRequestData });
  }

  onConfirmRejectCoach = async () => {
    await PostAsync(`/groupUser/reject`);
    var info = await GetAsync("/userInfo/get");
    this.setState({ userInfo: info });
  }

  rejectCoach = async () => {
    const lngStr = this.props.lngStr;

    var modalInfo = {
      isVisible: true,
      headerText: lngStr('modal.confirm'),
      buttons: [{ name: lngStr('button.confirm'), onClick: this.onConfirmRejectCoach, color: "success" }],
      body: () => { return (<p>{lngStr('user.confirmTrainerRejection')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  confirmAsync = async () => {
    await this.props.updateUserInfo(this.state.userInfo);
    await PostAsync(`/userAchivement/create`, [this.state.pushAchivement, this.state.jerkAchivement]);
    this.props.navigate("/");
  }

  render() {
    const lngStr = this.props.lngStr;

    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <h5 className="spaceBottom">{lngStr('user.cabinet')}</h5>
        {this.personalInfoPanel(lngStr)}

        <hr style={{ width: '75%', paddingTop: "2px", marginBottom: '30px' }} />

        <p>{lngStr('user.sportAchivements')}</p>
        {this.sportInfoPanel(lngStr)}

        <Button className="spaceTop" color="primary" onClick={() => this.confirmAsync()}>{lngStr('button.confirm')}</Button>
      </>
    );
  }

  personalInfoPanel(lngStr) {
    return (
      <>
        <Row className="spaceBottom" style={{ marginTop: '10px' }}>
          <Col xs={3}>
            <InputText label={lngStr('user.name') + ':'} propName="firstName" onChange={this.onValueChange} initialValue={this.state.userInfo.firstName} />
          </Col>
          <Col xs={3}>
            <InputText label={lngStr('user.surname') + ':'} propName="surname" onChange={this.onValueChange} initialValue={this.state.userInfo.surname} />
          </Col>
          <Col xs={3}>
            <InputText label={lngStr('user.patronimic') + ':'} propName="patronimic" onChange={this.onValueChange} initialValue={this.state.userInfo.patronimic} />
          </Col>
        </Row>
        <Row className="spaceBottom">
          <Col xs={3}>
            <InputNumber label={lngStr('user.weight') + ':'} propName="weight" onChange={this.onValueChange} initialValue={this.state.userInfo.weight} />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('user.height') + ':'} propName="height" onChange={this.onValueChange} initialValue={this.state.userInfo.height} />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('user.age') + ':'} propName="age" onChange={this.onValueChange} initialValue={this.state.userInfo.age} />
          </Col>
        </Row>

        {(this.state.userInfo?.rolesInfo?.isCoach || false) &&
          <Row className="spaceBottom">
            <Col xs={3}>
              <InputCheckbox label={lngStr('user.notTraining')} propName="coachOnly" onChange={this.onValueChange} initialValue={this.state.userInfo.coachOnly} />
            </Col>
          </Row>
        }
      </>
    );
  }

  sportInfoPanel(lngStr) {
    return (
      <>
        <Row style={{ marginTop: '10px' }}>
          <Col xs={3}>
            <InputNumber label={lngStr('user.pushAchivement') + ':'} propName="result" onChange={this.onPushChange} initialValue={this.state.pushAchivement?.result} />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('user.jerkAchivement') + ':'} propName="result" onChange={this.onJerkChange} initialValue={this.state.jerkAchivement?.result} />
          </Col>
        </Row>

        <Row className="spaceTop">
          <Col xs={6}>
            <Label check>
              <span style={{ marginRight: '20px' }} >{lngStr('user.myTrainer')}:</span>
              {this.coachRequestView(lngStr)}
            </Label>
          </Col>
        </Row>

      </>
    );
  }

  coachRequestView = (lngStr) => {
    if (this.state.userInfo.coachLegalName) {
      return (<>
        <strong className="spaceRight" >{this.state.userInfo.coachLegalName}</strong>
        <Button color="primary" onClick={() => this.rejectCoach()}>{lngStr('button.reject')}</Button >
      </>);
    }

    if (this.state.trainingRequest.coachName) {
      return (<>
        <strong className="spaceRight" >{lngStr('user.requestForTrainer')} {this.state.trainingRequest.coachName}</strong>
        <Button color="primary" onClick={() => this.cancelRequest()}>{lngStr('button.cancel')}</Button >
      </>
      );
    }

    return (<Button color="primary" onClick={() => this.createRequest()}>{lngStr('button.select')}</Button>);
  }

}

export default WithRouter(connect(null, mapDispatchToProps)(UserCabinet))