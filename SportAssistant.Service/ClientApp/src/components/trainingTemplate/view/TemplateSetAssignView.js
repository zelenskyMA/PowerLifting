﻿import React, { Component } from 'react';
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel, TableControl, DropdownControl } from "../../../common/controls/CustomControls";
import { Locale, DateToUtc } from "../../../common/LocalActions";
import WithRouter from "../../../common/extensions/WithRouter";

class TemplateSetAssignView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      groupInfo: Object,
      templateSets: [],
      templatePlans: [],
      selectedInfo: { setId: 0, templateId: 0, groupId: this.props.params.groupId, startDate: new Date() },
      selectedTemplateName: '',
      loading: true,
      error: '',
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var groupData = await GetAsync(`/trainingGroups/${this.props.params.groupId}`);
    var templateSets = await GetAsync("/templateSet/getList");
    this.setState({ groupInfo: groupData, templateSets: templateSets, loading: false });
  }

  goBack = () => { this.props.navigate(`/trainingGroup/${this.props.params.groupId}`); }
  onSelectionChange = (propName, value) => { this.setState(prevState => ({ error: '', selectedInfo: { ...prevState.selectedInfo, [propName]: value } })); }

  onSetSelect = async (id) => {
    var templateSetData = await GetAsync(`/templateSet/${id}`);

    this.onSelectionChange("setId", id);
    this.setState({ templatePlans: templateSetData.templates || [] });
  }

  onTemplateSelect = async (row) => {
    this.onSelectionChange("templateId", row.values.id);
    this.setState({ selectedTemplateName: row.values.name });
  }
  unselectTemplate = () => {
    this.onSelectionChange("templateId", 0);
    this.setState({ selectedTemplateName: '' });
  }

  onAssign = async () => {
    try {
      var request = this.state.selectedInfo;
      request.startDate = DateToUtc(this.state.selectedInfo.startDate);

      await PostAsync("/templateSet/assign", request);
      this.goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;
    var hasSets = this.state.templateSets && this.state.templateSets.length > 0;
    if (!hasSets) {
      return (<>
        <p className="spaceTop"><em>{lngStr('training.cycle.none')}</em></p>
        <Button color="primary" className="spaceTop" outline onClick={() => this.goBack()}>{lngStr('general.actions.back')}</Button>
      </>);
    }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('appSetup.user.name'), accessor: 'name' },
    ];

    var hasTemplates = this.state.templatePlans && this.state.templatePlans.length > 0;

    return (
      <div className="spaceTop">
        <ErrorPanel errorMessage={this.state.error} />

        <p>{lngStr('coaching.groups.toGroup')} <strong>{this.state.groupInfo.group.name}</strong> {lngStr('training.assignmentFrom')}:</p>
        <Calendar onChange={(date) => this.onSelectionChange('startDate', date)} value={this.state.selectedInfo.startDate} locale={Locale} />

        <Row className="spaceBottom spaceTop">
          <Col xs={8}>
            <DropdownControl placeholder={lngStr('general.common.notSet')} label={`${lngStr('training.cycle.byCycle')}: `} data={this.state.templateSets} onChange={this.onSetSelect} />
          </Col>
        </Row>

        {!hasTemplates && (<p><em>{lngStr('training.cycle.noTemplates')}</em></p>)}
        {hasTemplates && (
          <Row>
            <Col xs={9}>
              <p>{lngStr('training.template.singleSelection')}</p>
              <TableControl columnsInfo={columns} data={this.state.templatePlans} rowClick={this.onTemplateSelect} hideFilter="true" />
            </Col>
          </Row>)}

        {hasTemplates && this.state.selectedInfo.templateId > 0 && (
          <Row>
            <Col xs={1}><Button color="primary" outline onClick={() => this.unselectTemplate()}>{lngStr('general.actions.cancel')}</Button></Col>
            <Col>{lngStr('training.template.singleAssignment')}: {this.state.selectedTemplateName}</Col>
          </Row>)}

        <Button color="primary" className="spaceTop spaceRight" disabled={!(hasSets && hasTemplates)} onClick={() => this.onAssign()}>{lngStr('general.actions.assign')}</Button>
        <Button color="primary" className="spaceTop" outline onClick={() => this.goBack()}>{lngStr('general.actions.back')}</Button>
      </div>
    );
  }
}

export default WithRouter(TemplateSetAssignView);