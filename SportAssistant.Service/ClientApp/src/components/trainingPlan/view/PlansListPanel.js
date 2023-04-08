import React, { Component } from 'react';
import { connect } from "react-redux";
import { Button } from "reactstrap";
import { GetAsync, PostAsync } from "../../../common/ApiActions";
import { ErrorPanel, LoadingPanel, TabControl, TableControl, Tooltip } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import { DateToLocal } from "../../../common/LocalActions";
import { SaveExcelFile } from "../../../common/DownloadActions";
import { changeModalVisibility } from "../../../stores/appStore/appActions";

const mapDispatchToProps = dispatch => {
  return {
    changeModalVisibility: (modalInfo) => changeModalVisibility(modalInfo, dispatch)
  }
}

/*Используется в нескольких компонентах */
class PlansListPanel extends Component {
  constructor(props) {
    super(props);

    this.state = {
      activePlans: [],
      expiredPlans: [],
      loading: true,
      error: '',
    };
  }

  componentDidMount() { this.getInitData(); }

  getInitData = async () => {
    try {
      var plans = await GetAsync(`/trainingPlan/getList/${this.props.groupUserId}`);

      this.setState({ activePlans: plans.activePlans, expiredPlans: plans.expiredPlans, loading: false });
    }
    catch (error) { this.setState({ error: error.message, loading: false }); }
  }

  onRowClick = async (row) => { this.props.navigate(`/editPlanDays/${row.values.id}`); }

  confirmAsync = async () => { this.props.navigate("/"); }

  onDownloadAction = async (e, planId, planFileName, lngStr) => {
    e.stopPropagation();

    var modalInfo = {
      isVisible: true,
      headerText: lngStr('appSetup.modal.confirm'),
      buttons: [
        {
          name: lngStr('report.downloadAll'),
          onClick: async () => {
            var report = await PostAsync(`/reports/generate`, { planId: planId, completedOnly: false });
            SaveExcelFile(planFileName, report);
          },
          color: "success"
        },
        {
          name: lngStr('report.downloadCompleted'),
          onClick: async () => {
            var report = await PostAsync(`/reports/generate`, { planId: planId, completedOnly: true });
            SaveExcelFile(planFileName, report);
          },
          color: "success"
        },
      ],
      body: () => { return (<p>{lngStr('report.selectDataForExport')}</p>) }
    };

    this.props.changeModalVisibility(modalInfo);
  }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }
    const lngStr = this.props.lngStr;

    const columns = [
      { Header: 'Id', id: 'id', accessor: 'id' },
      { Header: '', id: 'download', accessor: 'id', Cell: t => this.downloadCell(t, t.value, lngStr) },
      { Header: lngStr('training.startDate'), accessor: 'startDate', Cell: t => DateToLocal(t.value) },
      { Header: lngStr('training.endDate'), accessor: 'finishDate', Cell: t => DateToLocal(t.value) }
    ];

    return (
      <>
        <ErrorPanel errorMessage={this.state.error} />

        <TabControl data={[
          { id: 1, label: lngStr('training.active'), renderContent: () => this.activePlansContent(columns, lngStr) },
          { id: 2, label: lngStr('training.history'), renderContent: () => this.historyPlansContent(columns) }
        ]}
        />
      </>
    );
  }

  downloadCell = (record, planId, lngStr) => {
    var planFileName = lngStr('report.exportPlanOf') + DateToLocal(record.row.original.startDate);

    return (
      <>
        <img id={'downloadPlan' + planId} src="/img/download_icon.png" width="25" height="23" className="rounded mx-auto d-block"
          onClick={(e) => this.onDownloadAction(e, planId, planFileName, lngStr)} />
        <Tooltip text={lngStr('report.export') + ' ' + planFileName} tooltipTargetId={'downloadPlan' + planId} />
      </>
    )
  }

  activePlansContent = (columns, lngStr) => {
    return (
      <>
        <TableControl columnsInfo={columns} data={this.state.activePlans} rowClick={this.onRowClick} pageSize={10} hideFilter={true} />
        <Button style={{ marginTop: '20px' }} color="primary" onClick={() => this.props.navigate(`/createPlan/${this.props.groupUserId}`)}>
          {lngStr('training.planTraining')}
        </Button>
      </>
    );
  }

  historyPlansContent = (columns) => {
    return (
      <TableControl columnsInfo={columns} data={this.state.expiredPlans} rowClick={this.onRowClick} pageSize={10} hideFilter={true} />
    );
  }
}

export default WithRouter(connect(null, mapDispatchToProps)(PlansListPanel));