import { Col, Row, Alert } from 'reactstrap';

export function InfoPanel({ infoMessage }) {
  if (infoMessage == '') {
    return (<></>);
  }

  return (
    <Row>
      <Col xs={6}>
        <Alert> {infoMessage} </Alert>
      </Col>
    </Row>
  );
}