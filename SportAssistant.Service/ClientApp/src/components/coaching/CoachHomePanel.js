import React, { Component } from 'react';
import { LoadingPanel } from "../../common/controls/CustomControls";
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';

class CoachHomePanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      loading: true
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {    
    this.setState({ loading: false });
  }
 
  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
  
    return (
      <>Экран тренера</>
    );
  }
}

export default WithRouter(CoachHomePanel);