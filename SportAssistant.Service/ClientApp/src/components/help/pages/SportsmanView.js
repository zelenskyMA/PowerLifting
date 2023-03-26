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

export function Usr_Header({ }) {
  return (
    <>
      {GetAsHtml('topMenuItem.sportsman.header1')}
      <div className="spaceMinTop">
        {GetAsHtml('topMenuItem.sportsman.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.sportsman.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.sportsman.step2')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('topMenuItem.sportsman.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/xm4eycOJywU')}</div>
    </>
  );
}

export function Usr_ExerciseCreation({ }) {
  return (
    <>
      {GetAsHtml('sportsman.exerciseCreation.header1')}
      <div className="spaceTop">
        {GetAsHtml('sportsman.exerciseCreation.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.exerciseCreation.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.exerciseCreation.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.exerciseCreation.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.exerciseCreation.step4')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.exerciseCreation.step5')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('sportsman.exerciseCreation.footer1')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.exerciseCreation.footer2')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/UsuxqVWl5ac')}</div>
    </>
  );
}

export function Usr_Planning({ }) {
  return (
    <>
      {GetAsHtml('sportsman.planning.header')}
      <div className="spaceTop">
        {GetAsHtml('sportsman.planning.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step4')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step5')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step6')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step7')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step8')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.planning.step9')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('sportsman.planning.footer1')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.planning.footer2')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/H_BtuiClZFk')}</div>
    </>
  );
}

export function Usr_OfpPlanning({ }) {
  return (
    <>
      {GetAsHtml('sportsman.ofpPlanning.header1')}
      <div className="spaceMinTop">{GetAsHtml('sportsman.ofpPlanning.header2')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.ofpPlanning.footer')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/q1rm9VNSmiw')}</div>
    </>
  );
}

export function Usr_CancelAndTransfer({ }) {
  return (
    <>
      {GetAsHtml('sportsman.cancelAndTransfer.header')}
      <div className="spaceTop">
        {GetAsHtml('sportsman.cancelAndTransfer.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.cancelAndTransfer.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.cancelAndTransfer.step2')}</div>
        <div className="space2Left spaceMinTop">{GetAsHtml('sportsman.cancelAndTransfer.step2a')}</div>
        <div className="space2Left spaceMinTop">{GetAsHtml('sportsman.cancelAndTransfer.step2b')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.cancelAndTransfer.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.cancelAndTransfer.step4')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('sportsman.cancelAndTransfer.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/wgAI27nP4OM')}</div>
    </>
  );
}

export function Usr_CompliteTraining({ }) {
  return (
    <>
      {GetAsHtml('sportsman.compliteTraining.header1')}
      <div className="spaceMinTop">{GetAsHtml('sportsman.compliteTraining.header2')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.compliteTraining.header3')}</div>
      <div className="spaceTop">
        {GetAsHtml('sportsman.compliteTraining.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.compliteTraining.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.compliteTraining.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.compliteTraining.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.compliteTraining.step4')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.compliteTraining.step5')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('sportsman.compliteTraining.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/zTF7Q4hhgGQ')}</div>
    </>
  );
}

export function Usr_TrainingAnalitics({ }) {
  return (
    <>
      {GetAsHtml('sportsman.trainingAnalitics.header')}
      <div className="spaceTop">
        {GetAsHtml('sportsman.trainingAnalitics.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.trainingAnalitics.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.trainingAnalitics.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.trainingAnalitics.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('sportsman.trainingAnalitics.footer1')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.trainingAnalitics.footer2')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.trainingAnalitics.footer3')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/yQE889Rx6b4')}</div>
    </>
  );
}

export function Usr_WorkWithCoach({ }) {
  return (
    <>
      {GetAsHtml('sportsman.workWithCoach.header1')}
      <div className="spaceMinTop">{GetAsHtml('sportsman.workWithCoach.header2')}</div>
      <div className="spaceTop">
        {GetAsHtml('sportsman.workWithCoach.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.workWithCoach.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.workWithCoach.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.workWithCoach.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('sportsman.workWithCoach.step1')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('sportsman.workWithCoach.footer1')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.workWithCoach.footer2')}</div>
      <div className="spaceMinTop">{GetAsHtml('sportsman.workWithCoach.footer3')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/bcMZ8-akMvU')}</div>
    </>
  );
}
