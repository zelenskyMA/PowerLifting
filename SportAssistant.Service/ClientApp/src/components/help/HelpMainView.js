import React, { Component } from 'react';
import { Navigation } from 'react-minimal-side-navigation';
import 'react-minimal-side-navigation/lib/ReactMinimalSideNavigation.css';
import { Col, Row } from 'reactstrap';
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import '../../styling/Custom.css';
import {
  Usr_Header, Usr_ExerciseCreation, Usr_Planning, Usr_CancelAndTransfer, Usr_CompliteTraining,
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
        {pageId === 1 && <Usr_ExerciseCreation />}
        {pageId === 2 && <Usr_Planning />}
        {pageId === 3 && <Usr_CancelAndTransfer />}
        {pageId === 4 && <Usr_CompliteTraining />}
        {pageId === 5 && <Usr_TrainingAnalitics />}
        {pageId === 6 && <Usr_WorkWithCoach />}

        {pageId === 20 && <Coach_Header />}
        {pageId === 21 && <Coach_MenuSetup />}
        {pageId === 22 && <Coach_GroupSetup />}
        {pageId === 23 && <Coach_RequestManagement />}
        {pageId === 24 && <Coach_ManageUserInGroup />}
        {pageId === 25 && <Coach_BeforePlanning />}
        {pageId === 26 && <Coach_SetPlanning />}
        {pageId === 27 && <Coach_TrainingAssignment />}
        {pageId === 28 && <Coach_TrainingCorrection />}
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
                { itemId: '/help/1', title: lngStr('help:menuItem.sportsman.exerciseCreation') },
                { itemId: '/help/2', title: lngStr('help:menuItem.sportsman.planning') },
                { itemId: '/help/3', title: lngStr('help:menuItem.sportsman.cancelAndTransfer') },
                { itemId: '/help/4', title: lngStr('help:menuItem.sportsman.compliteTraining') },
                { itemId: '/help/5', title: lngStr('help:menuItem.sportsman.trainingAnalitics') },
                { itemId: '/help/6', title: lngStr('help:menuItem.sportsman.workWithCoach') },
              ],
            },
            {
              title: lngStr('help:topMenuItem.toCoach'),
              itemId: '/help/20',
              //elemBefore: () => <Icon name="category" />,
              subNav: [
                { itemId: '/help/21', title: lngStr('help:menuItem.coach.menuSetup') },
                { itemId: '/help/22', title: lngStr('help:menuItem.coach.groupSetup') },
                { itemId: '/help/23', title: lngStr('help:menuItem.coach.requestManagement') },
                { itemId: '/help/24', title: lngStr('help:menuItem.coach.manageUserInGroup') },
                { itemId: '/help/25', title: lngStr('help:menuItem.coach.beforePlanning') },
                { itemId: '/help/26', title: lngStr('help:menuItem.coach.setPlanning') },
                { itemId: '/help/27', title: lngStr('help:menuItem.coach.trainingAssignment') },
                { itemId: '/help/28', title: lngStr('help:menuItem.coach.trainingCorrection') },
              ],
            },
          ]}
        />
      </>
    );
  }

}

export default WithRouter(HelpMainView);