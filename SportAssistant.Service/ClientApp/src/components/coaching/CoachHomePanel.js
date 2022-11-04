import React, { Component } from 'react';
import { Col, Container, Row } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import { LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class CoachHomePanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      groupExercises: [],
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    var data = await GetAsync("/trainingGroups/getExercisesList");

    this.setState({ groupExercises: data, loading: false });
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const groups = this.state.groupExercises;

    return (
      <>
        <Row>
          <Col xs={4}>{groups.length > 0 && this.groupPanel(groups[0])}</Col>
          <Col xs={4}>{groups.length > 1 && this.groupPanel(groups[1])}</Col>
          <Col xs={4}>{groups.length > 2 && this.groupPanel(groups[2])}</Col>
        </Row>
        <Row style={{ marginTop: '100px' }}>
          <Col xs={4}>{groups.length > 3 && this.groupPanel(groups[3])}</Col>
          <Col xs={4}>{groups.length > 4 && this.groupPanel(groups[4])}</Col>
          <Col xs={4}>{groups.length > 5 && this.groupPanel(groups[5])}</Col>
        </Row>
        <Row style={{ marginTop: '100px' }}>
          <Col xs={4}>{groups.length > 6 && this.groupPanel(groups[6])}</Col>
          <Col xs={4}>{groups.length > 7 && this.groupPanel(groups[7])}</Col>
          <Col xs={4}>{groups.length > 8 && this.groupPanel(groups[8])}</Col>
        </Row>
      </>
    );
  }

  groupPanel(group) {
    return (
      <Container fluid>
        <Row>
          <Col className="text-center">
            <div><strong>{group.name}</strong></div>
          </Col>
        </Row>
        <hr style={{ width: '100%', paddingTop: "2px" }} />
        <Row>
          <Col>{this.countersPanel(group.exerciseTypeCounters)}</Col>
        </Row>
      </Container>
    );
  }

  countersPanel(counters) {
    var fontSize = "0.85rem";

    if (counters.length == 0) {
      return (
        <span style={{ fontSize: fontSize }}>Нет назначенных упражнений</span>
      );
    }

    return (
      <>
        {counters.map((item, i) =>
          <Row key={i} style={{ fontSize: fontSize }}>
            <Col>{item.name}</Col>
            <Col>{item.value}</Col>
          </Row>
        )}
      </>
    );
  }
}

export default WithRouter(CoachHomePanel);