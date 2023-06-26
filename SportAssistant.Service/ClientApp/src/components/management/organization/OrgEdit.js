import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Row } from "reactstrap";
import { DeleteAsync, GetAsync, PostAsync, PutAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputNumber, InputText, InputTextArea, LoadingPanel, UserSearchControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { changeModalVisibility } from "../../../stores/appStore/appActions";
import '../../../styling/Common.css';

const mapStateToProps = store => { return { userInfo: store.app.myInfo, } }
const mapDispatchToProps = dispatch => { return { changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch) } }

class OrgEdit extends Component {
  constructor(props) {
    super(props);

    this.state = {
      orgInfo: Object,
      checkedUser: Object,

      backUrl: '',
      loading: true,
      error: '',
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    const [orgInfo] = await Promise.all([
      GetAsync(`/organization/${this.props.params.id}`)
    ]);

    var url = orgInfo?.ownerId === this.props?.userInfo?.id ? '/' : '/adminConsole';

    this.setState({ orgInfo: orgInfo, backUrl: url, loading: false });
  }

  onOrgChange = (propName, value) => { this.setState(prevState => ({ error: '', orgInfo: { ...prevState.orgInfo, [propName]: value } })); }

  handleSearchResult = (userCard, errorText) => {
    if (errorText) {
      this.setState({ error: errorText });
      return;
    }

    this.onOrgChange('ownerId', userCard.userId);
    this.onOrgChange('ownerLegalName', userCard.baseInfo.LegalName);
    this.setState({ checkedUser: userCard, error: '' });
  }

  onConfirmAsync = async () => {
    try {
      this.props.params.id > 0 ?
        await PutAsync(`/organization`, this.state.orgInfo) :
        await PostAsync(`/organization`, this.state.orgInfo);

      this.props.navigate(this.state.backUrl);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onDeleteAsync = async (lngStr) => {
    var modalInfo = {
      isVisible: true,
      headerText: lngStr('appSetup.modal.confirm'),
      buttons: [{ name: lngStr('general.actions.confirm'), onClick: this.onConfirmDelete, color: "success" }],
      body: () => { return (<p>{lngStr('management.org.confirmDeletion')}</p>) }
    };
    this.props.changeModalVisibility(modalInfo);
  }

  onConfirmDelete = async () => {
    try {
      await DeleteAsync(`/organization/${this.props.params.id}`);
      this.props.navigate(this.state.backUrl);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    const lngStr = this.props.lngStr;
    const editMode = this.props.params.id > 0;

    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <h5 className="spaceBottom">{lngStr('management.org.addOrUpdate')}</h5>
        <ErrorPanel errorMessage={this.state.error} />

        <Row>
          <Col xs={4}>
            <InputText label={lngStr('general.common.name')} propName="name" onChange={this.onOrgChange} initialValue={this.state.orgInfo.name} />
          </Col>
        </Row>
        <Row className="spaceTop">
          <Col xs={8}>
            <InputTextArea label={lngStr('general.common.description')} propName="description" rows="3" cols="80" onChange={this.onOrgChange} initialValue={this.state.orgInfo.description} />
          </Col>
        </Row>

        {editMode &&
          <Row>
            <Col xs={4} className="spaceTop">
              {this.props.userInfo?.rolesInfo?.isAdmin ?
                <InputNumber label={lngStr('management.org.maxCoaches')} propName="maxCoaches" onChange={this.onOrgChange} initialValue={this.state.orgInfo.maxCoaches} />
                :
                (lngStr('management.org.maxCoaches') + ': ' + this.state.orgInfo?.maxCoaches ?? 0)
              }
            </Col>
          </Row>
        }

        <p className="spaceTop">{lngStr('management.org.owner')}{': '}</p>
        <UserSearchControl handleSearchResult={this.handleSearchResult} userName={this.state.orgInfo?.ownerLegalName} />

        {!this.state.checkedUser?.baseInfo ?
          (<></>) :
          (<p className="spaceTopXs">
            <span className="spaceRight">
              {lngStr('management.org.userExists')}:
            </span>
            <b>
              {this.state.checkedUser?.baseInfo?.surname}{' '}{this.state.checkedUser?.baseInfo?.firstName}{' '}{this.state.checkedUser?.baseInfo?.patronimic}
            </b>
          </p>)
        }

        <Button className="spaceTop" color="primary" outline onClick={() => this.props.navigate(this.state.backUrl)}>{lngStr('general.actions.back')}</Button>
        <Button className="spaceTop spaceLeft" color="primary" onClick={() => this.onConfirmAsync()}>{lngStr('general.actions.confirm')}</Button>
        {editMode &&
          <Button className="spaceTop spaceLeft" color="primary" onClick={() => this.onDeleteAsync(lngStr)}>{lngStr('general.actions.delete')}{' '}{lngStr('management.org.deleteOrg')}</Button>
        }
      </>
    );
  }

}

export default WithRouter(connect(mapStateToProps, mapDispatchToProps)(OrgEdit))