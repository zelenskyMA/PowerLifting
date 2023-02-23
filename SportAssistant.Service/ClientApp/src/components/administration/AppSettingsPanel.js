import React, { Component } from 'react';
import { Col, Row, Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../common/ApiActions";
import { ErrorPanel, InfoPanel, InputNumber } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class AppSettingsPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      settings: Object,
      error: '',
      success: ''
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var settingsData = await GetAsync("/appSettings");
    this.setState({ settings: settingsData });
  }

  onValueChange = (propName, value) => {
    this.setState(prevState =>
      ({ error: '', success: '', settings: { ...prevState.settings, [propName]: value } }));
  }

  confirmAsync = async (lngStr) => {
    try {
      await PostAsync(`/appSettings`, { settings: this.state.settings });

      this.setState({ success: lngStr('appSetup.admin.settingsChanged'), error: '' });
    }
    catch (error) {
      this.setState({ error: error.message, success: '' });
    }
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <>
        <p className="spaceTop">{lngStr('appSetup.admin.settings')}</p>
        <ErrorPanel errorMessage={this.state.error} />
        <InfoPanel infoMessage={this.state.success} />

        <Row className="spaceBottom">
          <Col xs={3}>
            <InputNumber label={lngStr('appSetup.admin.maxActivePlans')} propName="maxActivePlans" onChange={this.onValueChange} initialValue={this.state.settings.maxActivePlans} />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('appSetup.admin.maxExercises')} propName="maxExercises" onChange={this.onValueChange} initialValue={this.state.settings.maxExercises} />
          </Col>
          <Col xs={3}>
            <InputNumber label={lngStr('appSetup.admin.maxLiftItems')} propName="maxLiftItems" onChange={this.onValueChange} initialValue={this.state.settings.maxLiftItems} />
          </Col>
        </Row>

        <Button className="spaceTop" color="primary" onClick={() => this.confirmAsync(lngStr)}>{lngStr('general.actions.confirm')}</Button>
      </>
    );
  }
}

export default WithRouter(AppSettingsPanel)