import React, { Component } from 'react';
import { Button, Col, Row } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputText, LoadingPanel, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class TemplateSetView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      templateSet: Object,
      newTemplatePlan: { setId: parseInt(this.props.params.id, 10), name: '' },
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getGroupData(); }

  getGroupData = async () => {
    var templateSetData = await GetAsync(`/templateSet/get?id=${this.props.params.id}`);

    this.setState({ templateSet: templateSetData, loading: false });
  }

  onRowClick = async (row) => { this.props.navigate(`/editTemplatePlan/${row.values.id}`); }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', templateSet: { ...prevState.templateSet, [propName]: value } })); }
  onPlanChange = (propName, value) => { this.setState(prevState => ({ error: '', newTemplatePlan: { ...prevState.newTemplatePlan, [propName]: value } })); }

  onDeleteTemplateSet = async () => {
    try {
      await PostAsync("/templateSet/delete", { id: this.props.params.id });
      this.props.navigate(`/templateSetList`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onUpdateTemplateSet = async () => { await PostAsync("/templateSet/update", this.state.templateSet); }

  onCreateTemplatePlan = async () => {
    try {
      var templatePlanId = await PostAsync("/templatePlan/create", this.state.newTemplatePlan);
      this.props.navigate(`/editTemplatePlan/${templatePlanId}`);
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Имя', accessor: 'name' },
    ];

    var hasData = this.state.templateSet.templates && this.state.templateSet.templates.length > 0;

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop spaceBottom">
          <Col xs={5}>
            <InputText label="Название цикла" propName="name" onChange={this.onValueChange} initialValue={this.state.templateSet.name} />
          </Col>
          <Col xs={4}>
            <Button color="primary" className="spaceRight" onClick={() => this.onUpdateTemplateSet()}>Сохранить</Button>
            <Button color="primary" onClick={() => this.onDeleteTemplateSet()}>Удалить</Button>
          </Col>
        </Row>

        {!hasData && (<p><em>В цикле нет шаблонов</em></p>)}
        {hasData && (<TableControl columnsInfo={columns} data={this.state.templateSet.templates} rowClick={this.onRowClick} />)}

        <hr className="spaceTop" style={{ paddingTop: "2px" }} />
        <Row>
          <Col xs={8}>
            <InputText label="Название нового шаблона" propName="name" onChange={this.onPlanChange} initialValue={this.state.newTemplatePlan.name} />
          </Col>
          <Col xs={2}>
            <Button color="primary" onClick={() => this.onCreateTemplatePlan()}>Создать</Button>
          </Col>
        </Row>

        <Button color="primary" className="spaceTop" outline onClick={() => this.props.navigate('/templateSetList')}>Назад</Button>
      </>
    );
  }
}

export default WithRouter(TemplateSetView);