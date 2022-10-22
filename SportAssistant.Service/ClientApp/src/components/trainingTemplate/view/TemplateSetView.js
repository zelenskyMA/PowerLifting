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
      error: '',
      loading: true
    };
  }

  componentDidMount() { this.getGroupData(); }

  getGroupData = async () => {
    var templateSetData = await GetAsync(`/templateSet/get?id=${this.props.params.id}`);
    this.setState({ templateSet: templateSetData, loading: false });
  }

  onRowClick = async (row) => { alert('Переход к шаблону ' + row.values.id); /*this.props.navigate(`/groupUser/${row.values.id}`);*/ }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', templateSet: { ...prevState.templateSet, [propName]: value } })); }

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
    alert('Создание шаблона ');
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Имя', accessor: 'name' },
    ];

    var hasData = this.state.templateSet.plans && this.state.templateSet.plans.length > 0;

    return (
      <>
        <h5>{this.state.templateSet.name}</h5>
        <ErrorPanel errorMessage={this.state.error} />

        <Row className="spaceTop spaceBottom">
          <Col xs={5}>
            <InputText label="Название цикла" propName="name" onChange={this.onValueChange} initialValue={this.state.templateSet.name} />
          </Col>
          <Col xs={3}>
            <Button color="primary" onClick={() => this.onUpdateTemplateSet()}>Обновить</Button>
          </Col>
        </Row>

        {!hasData && (
          <>
            <p><em>В цикле нет тренировочных планов</em></p>
            <Button color="primary" className="spaceTop spaceRight" onClick={() => this.onDeleteTemplateSet()}>Удалить цикл</Button>
          </>
        )}
        {hasData && (
          <>
            <p>Список тренировочных планов</p>
            <TableControl columnsInfo={columns} data={this.state.templateSet.plans} rowClick={this.onRowClick} />
            <Button color="primary" className="spaceTop spaceRight" onClick={() => this.onCreateTemplatePlan()}>Удалить цикл</Button>
          </>
        )}        

        <Button color="primary" className="spaceTop" outline onClick={() => this.props.navigate('/templateSetList')}>Назад</Button>
      </>
    );
  }
}

export default WithRouter(TemplateSetView);