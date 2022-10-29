import React, { Component } from 'react';
import Calendar from "react-calendar";
import "react-calendar/dist/Calendar.css";
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel, TableControl } from "../../../common/controls/CustomControls";
import { Locale, DateToUtc } from "../../../common/Localization";
import WithRouter from "../../../common/extensions/WithRouter";

class TemplateSetAssignView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      group: Object,
      templateSets: [],
      templatePlans: [],
      selectedInfo: { setId: 0, templateId: 0, groupId: this.props.params.groupId, startDate: new Date() },
      loading: true,
      error: '',
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var groupData = await GetAsync(`/trainingGroups/get?id=${this.props.params.groupId}`);
    var templateSets = await GetAsync("/templateSet/getList");
    this.setState({ group: groupData, templateSets: templateSets, loading: false });
  }

  goBack = () => { this.props.navigate(`/trainingGroup/${row.values.groupId}`); }
  onSelectionChange = (propName, value) => { this.setState(prevState => ({ error: '', selectedInfo: { ...prevState.selectedInfo, [propName]: value } })); }

  onSetSelect = async (id) => {
    var templateSetData = await GetAsync(`/templateSet/get?id=${id}`);

    this.onSelectionChange("setId", id);
    this.setState({ templatePlans: templateSetData.templates });
  }

  onTemplateSelect = async (row) => { this.onSelectionChange("templateId", row.values.id); }

  onAssign = async () => {
    try {
      var request = this.state.selectedInfo;
      request.startDate = DateToUtc(this.state.selectedInfo.startDate);

      await PostAsync("/templateSet/assign", request);
      goBack();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    var hasSets = this.state.templateSets && this.state.templateSets.length > 0;
    if (!hasSets) {
      return (<>
        <p className="spaceTop"><em>У вас нет тренировочных циклов</em></p>
        <Button color="primary" className="spaceTop" outline onClick={() => this.goBack()}>Назад</Button>
      </>);
    }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Имя', accessor: 'name' },
    ];

    var hasTemplates = this.state.templatePlans && this.state.templateSet.templatePlans > 0;

    return (
      <div className="spaceTop">
        <h5>Назначение группе "{this.state.group.name}" тренировки по шаблону</h5>
        <p>Начиная с:</p>
        <Calendar onChange={(date) => this.onSelectionChange('startDate', date)} value={this.state.selectedInfo.startDate} locale={Locale} />

        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceBottom">
          <Col xs={8}>
            <DropdownControl placeholder="Не задано" label="Выберите тренировочный цикл: " data={this.state.templateSets} onChange={this.onSetSelect} />
          </Col>
        </Row>

        {!hasTemplates && (<p><em>В цикле нет шаблонов</em></p>)}
        {hasTemplates && (
          <>
            <p>При необходимости, выберите один шаблон для назначения группе. В противном случае будет назначен весь тренировочный цикл.</p>
            <TableControl columnsInfo={columns} data={this.state.templatePlans} rowClick={this.onTemplateSelect} />
          </>)}

        <Button color="primary" className="spaceTop spaceRight" disabled={!(hasSets && hasTemplates)} onClick={() => this.onAssign()}>Назначить</Button>
        <Button color="primary" className="spaceTop" outline onClick={() => goBack()}>Назад</Button>
      </div>
    );
  }
}

export default WithRouter(TemplateSetAssignView);