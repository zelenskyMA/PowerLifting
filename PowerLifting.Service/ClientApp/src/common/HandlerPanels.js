import { Col, Row, Alert } from 'reactstrap';

export function HandleErrorPanel({ errorMessage }) {
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