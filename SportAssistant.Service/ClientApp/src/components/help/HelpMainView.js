import React, { Component } from 'react';
import { Navigation } from 'react-minimal-side-navigation';
import 'react-minimal-side-navigation/lib/ReactMinimalSideNavigation.css';
import { Col, Row } from 'reactstrap';
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import '../../styling/Custom.css';
import * as Coach from "./pages/CoachHelp";
import * as Org from './pages/OrgHelp';
import * as Usr from "./pages/SportsmanHelp";

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
        {pageId === 0 && <Usr.Header />}
        {pageId === 10 && <Usr.ExerciseCreation />}
        {pageId === 20 && <Usr.Planning />}
        {pageId === 30 && <Usr.OfpPlanning />}
        {pageId === 40 && <Usr.CancelAndTransfer />}
        {pageId === 50 && <Usr.CompliteTraining />}
        {pageId === 60 && <Usr.TrainingAnalitics />}
        {pageId === 70 && <Usr.WorkWithCoach />}

        {pageId === 200 && <Coach.Header />}
        {pageId === 210 && <Coach.MenuSetup />}
        {pageId === 220 && <Coach.GroupSetup />}
        {pageId === 230 && <Coach.RequestManagement />}
        {pageId === 240 && <Coach.ManageUserInGroup />}
        {pageId === 250 && <Coach.BeforePlanning />}
        {pageId === 260 && <Coach.SetPlanning />}
        {pageId === 270 && <Coach.TrainingAssignment />}
        {pageId === 280 && <Coach.TrainingCorrection />}

        {pageId === 400 && <Org.Header />}
        {pageId === 410 && <Org.Owner />}
        {pageId === 420 && <Org.Manager />}
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
              itemId: '/help/200',
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
            {
              title: lngStr('help:topMenuItem.toOrg'),
              itemId: '/help/400',
              //elemBefore: () => <Icon name="category" />,
              subNav: [
                { itemId: '/help/410', title: lngStr('help:menuItem.org.owner') },
                { itemId: '/help/420', title: lngStr('help:menuItem.org.manager') },
              ],
            },
          ]}
        />
      </>
    );
  }

}

export default WithRouter(HelpMainView);