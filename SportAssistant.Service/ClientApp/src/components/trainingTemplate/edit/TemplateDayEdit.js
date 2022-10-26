import React, { Component } from 'react';
import { Button } from "reactstrap";
import { GetAsync } from "../../../common/ApiActions";
import { LoadingPanel } from "../../../common/controls/CustomControls";
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
    var data = await GetAsync(`/templateDay/get?id=${this.props.params.id}`);
    this.setState({ templateDay: data, loading: false });
  }

  editSettings = (templateExercise) => { this.props.navigate(`/editTemplateExerciseSettings/${this.props.params.templateId}/${templateExercise.id}`); }

  confirmAsync = () => { this.props.navigate(`/editTemplatePlan/${this.props.params.templateId}`); }

  render() {
    if (this.state.loading) { return (<LoadingPanel />); }

    return (
      <>
        <h4 className="spaceBottom">План тренировок на день {this.state.templateDay.dayNumber}</h4>

        <table className='table table-striped' aria-labelledby="tabelLabel">
          <thead>
            <tr>
              <th className="nameColumn" >Упражнение</th>
              {this.state.templateDay.percentages.map((item, i) => <th key={'planDayHeader' + i} className="text-center">{item.name}</th>)}
              <th className="intColumn text-center">КПШ</th>
              <th style={{ width: '110px' }} className=" intColumntext-center">Нагрузка %</th>
            </tr>
          </thead>
          <tbody>
            {this.state.templateDay.exercises.map((templateExercise, i) =>
              <tr key={'planTr' + i}>
                <td role="button" title="Запланировать" onClick={() => this.editSettings(templateExercise)}>
                  {templateExercise.exercise.name}
                </td>

                {this.state.templateDay.percentages.map(item =>
                  <td key={item.id} className="text-center">
                    <TemplateSettingsEditPanel percentage={item} settings={templateExercise.settings} />
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

        <Button className="spaceRight" color="primary" onClick={() => this.confirmAsync()}>Подтвердить</Button>
        <Button color="primary" outline onClick={() => this.props.navigate(`/editTemplateExercises/${this.props.params.templateId}/${this.props.params.id}`)}>Назад</Button>
      </>
    );
  }

}

export default WithRouter(TemplateDayEdit);