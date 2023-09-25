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

export function Header() {
  return (
    <>
      {GetAsHtml('topMenuItem.org.header1')}
      <div className="spaceMinTop">{GetAsHtml('topMenuItem.org.header2')}</div>
      <div className="spaceMinTop">
        {GetAsHtml('topMenuItem.org.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.org.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.org.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('topMenuItem.org.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('topMenuItem.org.footer1')}</div>
    </>
  );
}

export function Owner() {
  return (
    <>
      {GetAsHtml('org.owner.header1')}
      <div className="spaceTop">
        {GetAsHtml('org.owner.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step3')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step4')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step5')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step6')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.owner.step7')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('org.owner.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/PSmyZskOQk8')}</div>
    </>
  );
}

export function Manager() {
  return (
    <>
      {GetAsHtml('org.manager.header1')}
      <div className="spaceTop">
        {GetAsHtml('org.manager.stepsHeader')}
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.manager.step1')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.manager.step2')}</div>
        <div className="spaceLeft spaceMinTop">{GetAsHtml('org.manager.step3')}</div>
      </div>

      <div className="spaceTop">{GetAsHtml('org.manager.footer1')}</div>

      <div className="spaceTop">{GetMsgWithUrl('https://youtu.be/7JyCGjXZMIU')}</div>
    </>
  );
}
