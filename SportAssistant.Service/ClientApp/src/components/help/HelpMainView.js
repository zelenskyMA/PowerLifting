import React, { Component } from 'react';
import { Navigation } from 'react-minimal-side-navigation';
import 'react-minimal-side-navigation/lib/ReactMinimalSideNavigation.css';
import { Col, Row } from 'reactstrap';
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import '../../styling/Custom.css';
import {
  Usr_Header, Usr_ExerciseCreation, Usr_Planning, Usr_OfpPlanning, Usr_CancelAndTransfer, Usr_CompliteTraining,
  Usr_TrainingAnalitics, Usr_WorkWithCoach
} from "./pages/SportsmanView";
import {
  Coach_Header, Coach_MenuSetup, Coach_GroupSetup, Coach_RequestManagement, Coach_ManageUserInGroup,
  Coach_BeforePlanning, Coach_SetPlanning, Coach_TrainingAssignment, Coach_TrainingCorrection
} from "./pages/CoachView";

class HelpMainView extends Component {
  constructor() {
    super();
    this.state = {};
  }

  render() {
    const lngStr = this.props.lngStr;

    return (
      <Row>
        <Col xs={3}>
          {this.menuPanel(lngStr)}
        </Col>
        <Col xs={9}>
          {this.dataPanel()}
        </Col>
      </Row>
    );
  }

  dataPanel = () => {
    var pageId = parseInt(this.props.params.id, 10);

    return (
      <>
        {pageId === 0 && <Usr_Header />}
        {pageId === 10 && <Usr_ExerciseCreation />}
        {pageId === 20 && <Usr_Planning />}
        {pageId === 30 && <Usr_OfpPlanning />}        
        {pageId === 40 && <Usr_CancelAndTransfer />}
        {pageId === 50 && <Usr_CompliteTraining />}
        {pageId === 60 && <Usr_TrainingAnalitics />}
        {pageId === 70 && <Usr_WorkWithCoach />}

        {pageId === 200 && <Coach_Header />}
        {pageId === 210 && <Coach_MenuSetup />}
        {pageId === 220 && <Coach_GroupSetup />}
        {pageId === 230 && <Coach_RequestManagement />}
        {pageId === 240 && <Coach_ManageUserInGroup />}
        {pageId === 250 && <Coach_BeforePlanning />}
        {pageId === 260 && <Coach_SetPlanning />}
        {pageId === 270 && <Coach_TrainingAssignment />}
        {pageId === 280 && <Coach_TrainingCorrection />}
      </>
    );
  }

  menuPanel = (lngStr) => {
    return (
      <>
        <Navigation
          activeItemId="/help/0" // use your own router's api to get pathname
          onSelect={({ itemId }) => {
            this.props.navigate(itemId);
          }}
          items={[
            {
              title: lngStr('help:topMenuItem.toSportsman'),
              itemId: '/help/0',
              //elemBefore: () => <Icon name="category" />,
              subNav: [
                { itemId: '/help/10', title: lngStr('help:menuItem.sportsman.exerciseCreation') },
                { itemId: '/help/20', title: lngStr('help:menuItem.sportsman.planning') },
                { itemId: '/help/30', title: lngStr('help:menuItem.sportsman.ofpPlanning') },
                { itemId: '/help/40', title: lngStr('help:menuItem.sportsman.cancelAndTransfer') },
                { itemId: '/help/50', title: lngStr('help:menuItem.sportsman.compliteTraining') },
                { itemId: '/help/60', title: lngStr('help:menuItem.sportsman.trainingAnalitics') },
                { itemId: '/help/70', title: lngStr('help:menuItem.sportsman.workWithCoach') },
              ],
            },
            {
              title: lngStr('help:topMenuItem.toCoach'),
              itemId: '/help/20',
              //elemBefore: () => <Icon name="category" />,
              subNav: [
                { itemId: '/help/210', title: lngStr('help:menuItem.coach.menuSetup') },
                { itemId: '/help/220', title: lngStr('help:menuItem.coach.groupSetup') },
                { itemId: '/help/230', title: lngStr('help:menuItem.coach.requestManagement') },
                { itemId: '/help/240', title: lngStr('help:menuItem.coach.manageUserInGroup') },
                { itemId: '/help/250', title: lngStr('help:menuItem.coach.beforePlanning') },
                { itemId: '/help/260', title: lngStr('help:menuItem.coach.setPlanning') },
                { itemId: '/help/270', title: lngStr('help:menuItem.coach.trainingAssignment') },
                { itemId: '/help/280', title: lngStr('help:menuItem.coach.trainingCorrection') },
              ],
            },
          ]}
        />
      </>
    );
  }

}

export default WithRouter(HelpMainView);