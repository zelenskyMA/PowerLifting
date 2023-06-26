import React, { Component } from 'react';
import { Col, Row } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel, Tooltip } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class OrgHomePanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      organization: Object,
      orgInfo: Object,
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    const [orgData, orgInfoData] = await Promise.all([
      GetAsync("/organization"),
      GetAsync(`/organization/info`)
    ]);

    this.setState({ organization: orgData, orgInfo: orgInfoData, loading: false });
  }

  onEdit = () => { this.props.navigate(`/organization/edit/${this.state.organization.id}`); }

  render() {
    const lngStr = this.props.lngStr;

    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <div className="inlineBlock">
          <h4>{this.state.organization.name}</h4>
          <img id="searchUserImg" src="/img/edit_icon.png" width="20" height="20" style={{ marginTop: '7px' }} className="rounded mx-auto spaceLeft clickable" alt="" onClick={() => this.onEdit()} />
          <Tooltip text={lngStr('general.actions.edit')} tooltipTargetId="searchUserImg" />
        </div>

        <p className="spaceBottom">{this.state.organization.description}</p>

        <p><strong>{lngStr('management.org.owner') + ': '}</strong> {this.state.organization.ownerLegalName}</p>
        <p><strong>{lngStr('management.managers') + ': '}</strong> {this.state.orgInfo.managerCount}</p>

        <Row>
          <Col xs={1}><strong>{lngStr('management.license.main')}</strong></Col>
          <Col xs={2}>{lngStr('management.license.available') + ': ' + this.state.organization.maxCoaches}</Col>
          <Col xs={2}>{lngStr('management.license.distributed') + ': ' + this.state.orgInfo.distributedLicences}</Col>
          <Col xs={2}>{lngStr('management.license.used') + ': ' + this.state.orgInfo.usedLicenses}</Col>
        </Row>
      </>
    );
  }
}

export default WithRouter(OrgHomePanel);