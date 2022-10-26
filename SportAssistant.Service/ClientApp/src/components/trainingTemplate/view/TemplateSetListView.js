import React, { Component } from 'react';
import { Button, Col, Row, Container } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, InputText, LoadingPanel, TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";

class TemplateSetListView extends Component {
  constructor(props) {
    super(props);

    this.state = {
      templateSets: [],
      newSet: { name: '' },
      loading: true,
      error: '',
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var templateSets = await GetAsync("/templateSet/getList");
    this.setState({ templateSets: templateSets, loading: false });
  }

  createTemplateSet = async () => {
    try {
      await PostAsync("/templateSet/create", this.state.newSet);
      await this.getInitData();
    }
    catch (error) {
      this.setState({ error: error.message });
    }
  }

  onValueChange = (propName, value) => { this.setState(prevState => ({ error: '', newSet: { ...prevState.newSet, [propName]: value } })); }

  onRowClick = async (row) => { this.props.navigate(`/templateSet/${row.values.id}`); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    var hasData = this.state.templateSets && this.state.templateSets.length > 0;
    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: 'Название', accessor: 'name' },
      { Header: 'Кол-во недель в цикле', accessor: data => { return String(data.templates.length); } }
    ];

    return (
      <div className="spaceTop">
        {!hasData && (<p><em>У вас нет тренировочных циклов</em></p>)}
        {hasData && (
          <TableControl columnsInfo={columns} data={this.state.templateSets} rowClick={this.onRowClick} />
        )}

        <Container fluid>
          <hr style={{ paddingTop: "2px" }} />
          <ErrorPanel errorMessage={this.state.error} />

          <Row>
            <Col xs={8}>
              <InputText label="Название нового цикла" propName="name" onChange={this.onValueChange} initialValue={this.state.newSet.name} />
            </Col>
            <Col xs={2}>
              <Button color="primary" onClick={() => this.createTemplateSet()}>Создать</Button>
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}

export default WithRouter(TemplateSetListView);