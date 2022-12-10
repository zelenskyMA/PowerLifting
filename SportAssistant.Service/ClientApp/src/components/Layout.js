import React, { Component } from 'react';
import { connect } from "react-redux";
import { Container } from 'reactstrap';
import { initApp } from "../stores/appStore/appActions";
import { GetToken } from '../common/TokenActions';
import NavMenu from './main/NavMenu';
import GlobalModalDialogPanel from './main/GlobalModalDialogPanel';
import '../styling/Custom.css';

const mapDispatchToProps = dispatch => {
  return {
    initApp: () => initApp(dispatch)
  }
}

class Layout extends Component {
  constructor(props) {
    super(props);
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    if (GetToken() != null) {
      await this.props.initApp();
    }
  }

  render() {
    var bckGround = GetToken() == null ? 'first-page-bckg': '';

    return (
      <div className={bckGround}>
        <NavMenu />
        <GlobalModalDialogPanel />
        <Container>
          {this.props.children}
        </Container>
      </div>
    );
  }
}

export default connect(null, mapDispatchToProps)(Layout);