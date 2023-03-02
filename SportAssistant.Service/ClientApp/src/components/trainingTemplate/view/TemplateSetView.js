import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync, PutAsync, DeleteAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputText, LoadingPanel, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

const mapStateToProps = store => {
  return {
    userInfo: store.app.myInfo,
  }
}

class TemplateSetView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      templateSet: Object,
      newTemplatePlan: { setId: parseInt(this.props.params.id, 10), name: '' },
      backRedirectUrl: this.props.userInfo?.coachOnly ? `/templateSetList` : `/coachConsole`,
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getGroupData(); }

  getGroupData = async () => {
    var templateSetData = await GetAsync(`/templateSet/${this.props.params.id}`);

    this.setState({ templateSet: templateSetData, loading: false });
  }

  onRowClick = async (row) => { this.props.navigate(`/editTemplatePlan/${row.values.id}`); }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', templateSet: { ...prevState.templateSet, [propName]: value } })); }
  onPlanChange = (propName, value) => { this.setState(prevState => ({ error: '', newTemplatePlan: { ...prevState.newTemplatePlan, [propName]: value } })); }

  onDeleteTemplateSet = async () => {
    try {
      await DeleteAsync(`/templateSet/${this.props.params.id}`);
      this.props.navigate(this.state.backRedirectUrl);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onUpdateTemplateSet = async () => { await PutAsync("/templateSet", this.state.templateSet); }

  onCreateTemplatePlan = async () => {
    try {
      var templatePlanId = await PostAsync("/templatePlan", this.state.newTemplatePlan);
      this.props.navigate(`/editTemplatePlan/${templatePlanId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('appSetup.user.name'), accessor: 'name' },
    ];

    var hasData = this.state.templateSet.templates && this.state.templateSet.templates.length > 0;

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop spaceBottom">
          <Col xs={5}>
            <InputText label={lngStr('training.cycle.name')} propName="name" onChange={this.onValueChange} initialValue={this.state.templateSet.name} />
          </Col>
          <Col xs={4}>
            <Button color="primary" className="spaceRight" onClick={() => this.onUpdateTemplateSet()}>{lngStr('general.actions.save')}</Button>
            <Button color="primary" onClick={() => this.onDeleteTemplateSet()}>{lngStr('general.actions.delete')}</Button>
          </Col>
        </Row>

        {!hasData && (<p><em>{lngStr('training.cycle.noTemplates')}</em></p>)}
        {hasData && (<TableControl columnsInfo={columns} data={this.state.templateSet.templates} rowClick={this.onRowClick} />)}

        <hr className="spaceTop" style={{ paddingTop: "2px" }} />
        <Row>
          <Col xs={8}>
            <InputText label={lngStr('training.template.newName')} propName="name" onChange={this.onPlanChange} initialValue={this.state.newTemplatePlan.name} />
          </Col>
          <Col xs={2}>
            <Button color="primary" onClick={() => this.onCreateTemplatePlan()}>{lngStr('general.actions.create')}</Button>
          </Col>
        </Row>

        <Button color="primary" className="spaceTop" outline onClick={() => this.props.navigate(this.state.backRedirectUrl)}>{lngStr('general.actions.back')}</Button>
      </>
    );
  }
}

export default WithRouter(connect(mapStateToProps, null)(TemplateSetView))