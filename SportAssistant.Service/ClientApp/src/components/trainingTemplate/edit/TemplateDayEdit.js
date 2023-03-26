import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel, Tooltip } from "../../../common/controls/CustomControls";
import WithRouter from "../../../common/extensions/WithRouter";
import '../../../styling/Common.css';
import { TemplateSettingsEditPanel } from "./TemplateSettingsEditPanel";

class TemplateDayEdit extends Component {
  constructor() {
    super();

    this.state = {
      templateDay: Object,
      loading: true,
    };
  }

  componentDidMount() { this.getInitData(); }

  async getInitData() {
    var data = await GetAsync(`/templateDay/${this.props.params.id}`);
    this.setState({ templateDay: data, loading: false });
  }

  editSettings = (templateExercise) => {
    var exerciseTypeId = parseInt(templateExercise.exercise.exerciseTypeId, 10);
    switch (exerciseTypeId) {
      case 3: this.props.navigate(`/editTemplateOfpExercise/${this.props.params.templateId}/${templateExercise.id}`); break;
      default: this.props.navigate(`/editTemplateExerciseSettings/${this.props.params.templateId}/${templateExercise.id}`); break;
    }
  }

  confirmAsync = () => { this.props.navigate(`/editTemplatePlan/${this.props.params.templateId}`); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    const lngStr = this.props.lngStr;

    return (
      <>
        <h4 className="spaceBottom">{lngStr('training.plan.trainingDay') + ' ' + this.state.templateDay.dayNumber}</h4>

        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th className="nameColumn" >{lngStr('training.exercise.header')}</th>
              {this.state.templateDay.percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
              <th className="intColumn text-center">{lngStr('training.entity.liftCounter')}</th>
              <th style={{ width: '110px' }} className="intColumn text-center">{lngStr('training.entity.weightLoad')} %</th>
            </tr>
          </thead>
          <tbody>
            {this.state.templateDay.exercises.map((templateExercise, i) =>
              <tr key={'planTr' + i}>
                <td id={'exerciseName' + i} role="button" onClick={() => this.editSettings(templateExercise)}>
                  {templateExercise.exercise.name}
                </td>
                <Tooltip text={lngStr('general.actions.schedule')} tooltipTargetId={'exerciseName' + i} placement="left" />

                {this.state.templateDay.percentages.map(item =>
                  <td key={item.id} className="text-center">
                    <TemplateSettingsEditPanel percentage={item} templateExercise={templateExercise} />
                  </td>
                )}
                <td className="text-center"><strong>{templateExercise.liftCounter}</strong></td>
                <td className="text-center"><strong>{templateExercise.weightLoadPercentage}</strong></td>
              </tr>
            )}
          </tbody>
          <tfoot>
            <tr>
              <td></td>
              {this.state.templateDay.percentages.map(() => <td></td>)}
              <td className="text-center"><strong>{this.state.templateDay.liftCounterSum}</strong></td>
              <td className="text-center"><strong>{this.state.templateDay.weightLoadPercentageSum}</strong></td>
            </tr>
          </tfoot>
        </table>

        <Button className="spaceRight" color="primary" onClick={() => this.confirmAsync()}>{lngStr('general.actions.confirm')}</Button>
        <Button color="primary" outline onClick={() => this.props.navigate(`/editTemplateExercises/${this.props.params.templateId}/${this.props.params.id}`)}>{lngStr('general.actions.back')}</Button>
      </>
    );
  }

}

export default WithRouter(TemplateDayEdit);