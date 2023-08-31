import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { DropdownControl, ErrorPanel, LoadingPanel, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import '../../../styling/Common.css';

const mapDispatchToProps = dispatch => { return { changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch) } }

class AssignedCoachView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      coach: Object,

      orgManagers: [],
      toggleCoachTransfer: false,
      targetManagerId: 0,

      loading: true,
      error: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    const [coachData, managerList] = await Promise.all([
      GetAsync(`/assignedCoach/${this.props.params.id}`),
      GetAsync(`/manager/getList`)
    ]);

    var filteredList = managerList.filter((user) => user.id !== coachData.managerId)

    this.setState({ coach: coachData, orgManagers: filteredList, loading: false });
  }

  onManagerSelect = (id) => { this.setState({ error: '', targetManagerId: id }); }
  goBack = () => { this.props.navigate(`/assignedCoaches/list`); }
  toggleTransfer = () => { this.setState({ toggleCoachTransfer: !this.state.toggleCoachTransfer }); }

  confirmTransfer = async () => {
    var request = { sourceManagerId: this.state.coach.managerId, targetManagerId: this.state.targetManagerId, coachIds: [this.props.params.id] };

    try {
      await PostAsync("manager/transfer", request);
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
      body: () => { return (<p>{lngStr('management.assignedCoach.confirmUnAssign')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  onConfirmUnAssign = async () => {
    try {
      await PostAsync(`/assignedCoach/changeRole`, { userId: this.props.params.id, roleStatus: false });
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
        <h5>{this.state.coach?.coachName} ({lngStr('management.assignedCoach.coach')})</h5>
        <ErrorPanel errorMessage={this.state.error} />

        {this.personalInfoPanel(lngStr)}

        {this.transferCoachPanel(lngStr)}

        <hr style={{ width: '75%', paddingTop: "2px", marginBottom: '30px' }} />

        <div className="spaceTop">
          <Button color="primary" className="spaceRight" onClick={() => this.onUnAssignAsync(lngStr)}>{lngStr('management.unAssign')}</Button>
          <Button color="primary" outline onClick={() => this.goBack()}>{lngStr('general.actions.back')}</Button>
        </div>
      </>
    );
  }

  personalInfoPanel(lngStr) {
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('general.common.userName'), accessor: 'legalName' },
    ];

    if (!(this.state.coach && this.state.coach?.sportsmen?.length > 0)) { return (<p><em>{lngStr('management.assignedCoach.noSportsmen')}</em></p>); }

    return (
      <>
        <p>{lngStr('management.assignedCoach.coachSportsmen')}</p>
        <Container className="spaceMinTop">
          <TableControl columnsInfo={columns} data={this.state.coach?.sportsmen} pageSize={10} />
        </Container>
      </>
    );
  }

  transferCoachPanel(lngStr) {
    if (!this.state.orgManagers || this.state.orgManagers?.length === 0) { return (<></>); }

    return (
      <div className="spaceTop">
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

export default WithRouter(connect(null, mapDispatchToProps)(AssignedCoachView))