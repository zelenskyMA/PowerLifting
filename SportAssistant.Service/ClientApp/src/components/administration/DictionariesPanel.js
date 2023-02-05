import React, { Component } from 'react';
import { ErrorPanel } from "../../common/controls/CustomControls";
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
    const lngStr = this.props.lngStr;

    return (
      <>
        <p className="spaceTop">{lngStr('appSetup.admin.dictionaries')}</p>
        <ErrorPanel errorMessage={this.state.error} />
      </>
    );
  }

}

export default WithRouter(DictionariesPanel)