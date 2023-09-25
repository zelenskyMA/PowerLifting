import React from 'react';
import '../../../styling/Common.css';
import { GetAsHtml, GetMsgWithUrl } from "./CommonHelpActions";

/*
"header1": "",
"stepsHeader": "",
"step1": "",
"footer1": "",
"footer2": ""
*/

export function Header({ }) {
  return (
    <>
      {GetAsHtml('topMenuItem.coach.header1')}
      <div className="spaceMinTop">
        {GetAsHtml('topMenuItem.coach.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.coach.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.coach.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.coach.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.coach.step4')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('topMenuItem.coach.footer1')}</div>
    </>
  );
}

export function MenuSetup({ }) {
  return (
    <>
      {GetAsHtml('coach.menuSetup.header1')}
      <div className="spaceMinTop">
        {GetAsHtml('coach.menuSetup.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.menuSetup.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.menuSetup.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.menuSetup.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.menuSetup.footer1')}</div>
      <div className="spaceMinTop">{GetAsHtml('coach.menuSetup.footer2')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/fT9VOZWerug')}</div>
    </>
  );
}

export function GroupSetup({ }) {
  return (
    <>
      {GetAsHtml('coach.groupSetup.header1')}
      <div className="spaceMinTop">{GetAsHtml('coach.groupSetup.header2')}</div>
      <div className="spaceMinTop">
        {GetAsHtml('coach.groupSetup.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.groupSetup.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.groupSetup.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.groupSetup.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.groupSetup.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/CnpJs8fFcrQ')}</div>
    </>
  );
}

export function RequestManagement({ }) {
  return (
    <>
      {GetAsHtml('coach.requestManagement.header1')}
      <div className="spaceMinTop">{GetAsHtml('coach.requestManagement.header2')}</div>
      <div className="spaceMinTop">
        {GetAsHtml('coach.requestManagement.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.requestManagement.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.requestManagement.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.requestManagement.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.requestManagement.step4')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.requestManagement.step5')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.requestManagement.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/RPTS3_QsW9g')}</div>
    </>
  );
}

export function ManageUserInGroup({ }) {
  return (
    <>
      {GetAsHtml('coach.manageUserInGroup.header1')}
      <div className="spaceMinTop">
        {GetAsHtml('coach.manageUserInGroup.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.manageUserInGroup.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.manageUserInGroup.step2')}</div>
        <div className="space2Left spaceMinTop">{GetAsHtml('coach.manageUserInGroup.step2a')}</div>
        <div className="space2Left spaceMinTop">{GetAsHtml('coach.manageUserInGroup.step2b')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.manageUserInGroup.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.manageUserInGroup.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/E3Yqes0_C7o')}</div>
    </>
  );
}

export function BeforePlanning({ }) {
  return (
    <>
      {GetAsHtml('coach.beforePlanning.header1')}
      <div className="spaceMinTop">{GetAsHtml('coach.beforePlanning.header2')}</div>
    </>
  );
}

export function SetPlanning({ }) {
  return (
    <>
      <div>
        {GetAsHtml('coach.setPlanning.stepsHeader1')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step1_1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step2_1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step3_1')}</div>
      </div>

      <div className="spaceMinTop">
        {GetAsHtml('coach.setPlanning.stepsHeader2')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step1_2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step2_2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step3_2')}</div>
      </div>

      <div className="spaceMinTop">
        {GetAsHtml('coach.setPlanning.stepsHeader3')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step1_3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.setPlanning.step2_3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.setPlanning.footer1')}</div>
      <div className="spaceMinTop">{GetAsHtml('coach.setPlanning.footer2')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/EcO2E1TgycE')}</div>
    </>
  );
}

export function TrainingAssignment({ }) {
  return (
    <>
      <div>
        {GetAsHtml('coach.trainingAssignment.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingAssignment.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingAssignment.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingAssignment.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingAssignment.step4')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingAssignment.step5')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingAssignment.step6')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.trainingAssignment.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/wsziK3HIk84')}</div>
    </>
  );
}


export function TrainingCorrection({ }) {
  return (
    <>
      {GetAsHtml('coach.trainingCorrection.header1')}
      <div className="spaceMinTop">
        {GetAsHtml('coach.trainingCorrection.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingCorrection.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingCorrection.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('coach.trainingCorrection.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('coach.trainingCorrection.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/abKScQMZBBg')}</div>
    </>
  );
}