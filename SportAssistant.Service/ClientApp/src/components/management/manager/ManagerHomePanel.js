import React, { Component } from 'react';
import { Button, Col, Container, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { InfoPanel, InputText, LoadingPanel } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class ManagerHomePanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      manager: Object,
      organization: Object,
      loading: true,
      success: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var managerData = await GetAsync("/manager");
    var orgData = await GetAsync(`/organization/${managerData.orgId}`);

    this.setState({ manager: managerData, organization: orgData, loading: false, success: '' });
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ success: '', manager: { ...prevState.manager, [propName]: value } })); }
  onSaveChanges = async (lngStr) => {
    await PostAsync('manager', { manager: this.state.manager });
    this.setState({ success: lngStr('general.actions.successSave') });
  }

  render() {
    const lngStr = this.props.lngStr;

    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <h4>{lngStr('management.org.yourOrg') + ": " + this.state.organization.name}</h4>

        <p>{lngStr('management.yourInfo')}</p>
        <InfoPanel infoMessage={this.state.success} />

        <Container>

          {/*<Row>*/}
          {/*  <Col xs={3}>*/}
          {/*    <InputText label={lngStr('general.common.tel') + ':'} propName="telNumber" onChange={this.onValueChange} initialValue={this.state.manager.telNumber} />*/}
          {/*  </Col>*/}
          {/*</Row>*/}
          {/*<Row className="spaceTopXs">*/}
          {/*  <Col xs={3}>*/}
          {/*    <Button color="primary" onClick={() => this.onSaveChanges(lngStr)}>{lngStr('general.actions.save')}</Button>*/}
          {/*  </Col>*/}
          {/*</Row>*/}

          <Row className="spaceTop">
            <Col xs={1}><strong>{lngStr('management.license.main')}</strong></Col>
            <Col xs={2}>{lngStr('management.license.sum') + ': ' + this.state.manager.allowedCoaches}</Col>
            <Col xs={2}>{lngStr('management.license.available') + ': ' + (this.state.manager.allowedCoaches - this.state.manager.distributedCoaches)}</Col>
            <Col xs={2}>{lngStr('management.license.used') + ': ' + this.state.manager.distributedCoaches}</Col>
          </Row>
        </Container>     
      </>
    );
  }
}

export default WithRouter(ManagerHomePanel);