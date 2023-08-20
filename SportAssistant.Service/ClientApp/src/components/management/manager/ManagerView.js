import React, { Component } from 'react';
import { connect } from "react-redux";
import { Container, Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { InputNumber, InputText, LoadingPanel, ErrorPanel, UserSearchControl } from "../../../common/controls/CustomControls";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

const mapDispatchToProps = dispatch => { return { changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch) } }

class ManagerView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      manager: Object,
      checkedUser: Object,
      toggleCoachTransfer: false,

      loading: true,
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    const [managerData] = await Promise.all([
      GetAsync(`/manager/${this.props.params.id}`)
    ]);

    this.setState({ manager: managerData, loading: false });
  }

  handleSearchResult = (userCard, errorText) => {
    if (errorText) {
      this.setState({ error: errorText });
      return;
    }

    this.setState({ checkedUser: userCard, error: '' });
  }


  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', manager: { ...prevState.manager, [propName]: value } })); }
  goBack = () => { this.props.navigate(`/manager/list`); }
  toggleTransfer = () => { this.setState({ toggleCoachTransfer: !this.state.toggleCoachTransfer }); }

  onSaveChanges = async () => {
    try {
      await PostAsync('manager', { manager: this.state.manager });
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  confirmTransfer = async () => {
    try {
      await PostAsync("manager/transfer", { sourceManagerId: this.props.params.id, targetManagerId: this.state.checkedUser.userId });
      this.setState({ error: '' });
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
        <Row>
          <Col xs={2}><h5 className="spaceBottom">{lngStr('management.manager')}</h5></Col>
          <Col>{this.state.manager.Name}</Col>
        </Row>

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
            <InputText label={lngStr('general.common.tel') + ':'} propName="telNumber" onChange={this.onValueChange} initialValue={this.state.manager.telNumber} />
          </Col>
        </Row>
      </>
    );
  }

  transferCoachPanel(lngStr) {
    return (
      <div className="spaceMinTop">
        <Button color="primary" outline onClick={() => this.toggleTransfer()}>{lngStr('management.coachTransfer')}</Button>
        {this.state.toggleCoachTransfer && (
          <Container>
            <UserSearchControl className="spaceTopXs" handleSearchResult={this.handleSearchResult} />
            {!this.state.checkedUser?.userName ?
              (<></>) :
              (
                <>
                  <p className="spaceTopXs">
                    <span className="spaceRight"> {lngStr('management.org.userExists')}: </span>
                    <b> {this.state.checkedUser?.userName} </b>
                  </p>
                  <Button color="primary" onClick={() => this.confirmTransfer()}>{lngStr('general.actions.confirm')}</Button>
                </>
              )
            }
          </Container>
        )}

      </div>
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(ManagerView))