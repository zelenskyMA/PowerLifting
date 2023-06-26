import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { TableControl } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';

class OrgListPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      orgList: [],

      loading: false,
    };
  }

  componentDidMount() { this.getGroupData(); }

  getGroupData = async () => {
    var orgList = await GetAsync(`/organization/getList`);
    this.setState({ orgList: orgList, loading: false });
  }

  onRowClick = row => { this.props.navigate(`/organization/edit/${row.values.id}`); }

  render() {
    const lngStr = this.props.lngStr;

    const columns = [
      { Header: 'Id', accessor: 'id' },
      { Header: lngStr('general.common.name'), accessor: 'name' },
      { Header: lngStr('management.org.owner'), accessor: 'ownerLegalName' },      
      { Header: lngStr('management.org.maxCoaches'), accessor: 'maxCoaches' },
    ];

    var hasData = this.state.orgList && this.state.orgList.length > 0;

    return (
      <>
        <p className="spaceTop">{lngStr('management.org.list')}</p>

        {hasData ?
          <TableControl columnsInfo={columns} data={this.state.orgList} rowClick={this.onRowClick} /> :
          <p><em>{lngStr('management.org.emptyList')}</em></p>
        }

        <Button color="primary" className="spaceRight spaceTop" onClick={() => this.props.navigate(`/organization/edit/0`)}>{lngStr('general.actions.create')}</Button>
      </>
    );
  }
}

export default WithRouter(OrgListPanel)