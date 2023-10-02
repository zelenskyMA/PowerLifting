import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { DropdownControl, ErrorPanel, InputNumber, InputText, LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import '../../../styling/Common.css';

const mapDispatchToProps = dispatch => { return { changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch) } }

class ManagerView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      manager: Object,

      orgManagers: [],
      targetManagerId: 0,
      toggleCoachTransfer: false,

      loading: true,
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    const [managerData, managerList] = await Promise.all([
      GetAsync(`/manager/${this.props.params.id}`),
      GetAsync(`/manager/getList`)
    ]);

    var filteredList = managerList.filter((user) => user.id.toString() !== this.props.params.id)

    this.setState({ manager: managerData, orgManagers: filteredList, loading: false });
  }

  onManagerSelect = (id) => { this.setState({ error: '', targetManagerId: id }); }
  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', manager: { ...prevState.manager, [propName]: value } })); }
  goBack = () => { this.props.navigate(`/manager/list`); }
  toggleTransfer = () => { this.setState({ toggleCoachTransfer: !this.state.toggleCoachTransfer }); }

  onSaveChanges = async () => {
    try {
      var d = 0;

      await PostAsync('manager', { manager: this.state.manager });
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  confirmTransfer = async () => {
    try {
      await PostAsync("manager/transfer", { sourceManagerId: this.props.params.id, targetManagerId: this.state.targetManagerId });
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onUnAssignAsync = async (lngStr) => {
    var modalInfo = {
      isVisible: true,
      headerText: lngStr('appSetup.modal.confirm'),
      buttons: [{ name: lngStr('general.actions.confirm'), onClick: this.onConfirmUnAssign, color: "success" }],
      body: () => { return (<p>{lngStr('management.confirmUnAssign')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  onConfirmUnAssign = async () => {
    try {
      await PostAsync(`/manager/changeRole`, { userId: this.props.params.id, roleStatus: false });
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    const lngStr = this.props.lngStr;

    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <h5 className="spaceBottom">{this.state.manager.name} ({lngStr('management.manager')})</h5>

        <ErrorPanel errorMessage={this.state.error} />

        {this.personalInfoPanel(lngStr)}

        {this.transferCoachPanel(lngStr)}

        <hr style={{ width: '75%', paddingTop: "2px", marginBottom: '30px' }} />

        <div className="spaceTop">
          <Button color="primary" className="spaceRight" onClick={() => this.onUnAssignAsync(lngStr)}>{lngStr('management.unAssign')}</Button>
          <Button color="primary" className="spaceRight" onClick={() => this.onSaveChanges()}>{lngStr('general.actions.save')}</Button>
          <Button color="primary" outline onClick={() => this.goBack()}>{lngStr('general.actions.back')}</Button>
        </div>
      </>
    );
  }

  personalInfoPanel(lngStr) {
    return (
      <>
        <p>{lngStr('management.license.main')}</p>
        <Row className="spaceBottom" style={{ marginTop: '10px' }}>
          <Col xs={3}>
            <InputNumber label={lngStr('management.license.available') + ':'} propName="allowedCoaches" onChange={this.onValueChange} initialValue={this.state.manager.allowedCoaches} />
          </Col>
          <Col xs={3} className="spaceMinTop">
            <p>{lngStr('management.license.distributed') + ': ' + this.state.manager.distributedCoaches}</p>
          </Col>
        </Row>

        <Row className="spaceBottom">
          <Col xs={3}>
            <p>{lngStr('general.common.tel') + ': ' + this.state.manager.telNumber}</p>
          </Col>
        </Row>
      </>
    );
  }

  transferCoachPanel(lngStr) {
    if (!this.state.orgManagers || this.state.orgManagers?.length === 0) { return (<></>); }

    return (
      <div className="spaceMinTop">
        <Button color="primary" outline onClick={() => this.toggleTransfer()}>{lngStr('management.assignedCoach.coachTransfer')}</Button>
        {this.state.toggleCoachTransfer && (
          <Container className="spaceMinTop">
            <DropdownControl placeholder={lngStr('general.common.notSet')} label={lngStr('management.transferTarget') + ': '}
              data={this.state.orgManagers} onChange={this.onManagerSelect} />

            {!this.state.targetManagerId > 0 ?
              <></> :
              <Button className="spaceTopXs" color="primary" onClick={() => this.confirmTransfer()}>{lngStr('general.actions.confirm')}</Button>
            }
          </Container>
        )}

      </div>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(ManagerView))