import { Col, Row, Alert } from 'reactstrap';

export function ErrorPanel({ errorMessage }) {
  if (errorMessage == '') {
    return (<></>);
  }

  return (
    <Row>
      <Col xs={6}>
        <Alert color="danger"> {errorMessage} </Alert>
      </Col>
    </Row>
  );
}