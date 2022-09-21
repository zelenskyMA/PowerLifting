import React, { Component } from 'react';
import { Col, Row } from "reactstrap";
import { GetAsync } from "../../common/ApiActions";
import UserAdministrationPanel from "./UserAdministrationPanel"
import { ErrorPanel, InputNumber, InputText } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class DictionariesPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      error: ''
    };
  }

  render() {
    return (
      <>
        <p className="spaceTop">Справочники</p>
        <ErrorPanel errorMessage={this.state.error} />
      </>
    );
  }

}

export default WithRouter(DictionariesPanel)