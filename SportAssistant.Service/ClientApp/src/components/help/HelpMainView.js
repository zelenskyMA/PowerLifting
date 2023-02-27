import React, { Component } from 'react';
import { Navigation } from 'react-minimal-side-navigation';
import 'react-minimal-side-navigation/lib/ReactMinimalSideNavigation.css';
import { Col, Row } from 'reactstrap';
import WithRouter from "../../common/extensions/WithRouter";
import '../../styling/Common.css';
import '../../styling/Custom.css';
import {
  UsrHeaderView, UsrItemExerciseCreationView, UsrItemPlanningView, UsrItemCancelAndTransferView, UsrItemCompliteTrainingView,
  UsrItemTrainingAnaliticsView, UsrItemWorkWithCoachView
} from "./pages/SportsmanView";

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
          {this.dataPanel(lngStr)}
        </Col>
      </Row>
    );
  }

  dataPanel = (lngStr) => {
    var pageId = parseInt(this.props.params.id, 10);

    return (
      <>
        {pageId === 0 && <UsrHeaderView />}
        {pageId === 1 && <UsrItemExerciseCreationView />}
        {pageId === 2 && <UsrItemPlanningView />}
        {pageId === 3 && <UsrItemCancelAndTransferView />}
        {pageId === 4 && <UsrItemCompliteTrainingView />}
        {pageId === 5 && <UsrItemTrainingAnaliticsView />}
        {pageId === 6 && <UsrItemWorkWithCoachView />}

        {pageId === 20 && lngStr('topMenuItem.coach')}
        {pageId === 21 && "С начала."}
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
              title: 'Спортсмену',
              itemId: '/help/0',
              //elemBefore: () => <Icon name="category" />,
              subNav: [
                { itemId: '/help/1', title: lngStr('help:menuItem.exerciseCreation') },
                { itemId: '/help/2', title: lngStr('help:menuItem.planning') },
                { itemId: '/help/3', title: lngStr('help:menuItem.cancelAndTransfer') },
                { itemId: '/help/4', title: lngStr('help:menuItem.compliteTraining') },
                { itemId: '/help/5', title: lngStr('help:menuItem.trainingAnalitics') },
                { itemId: '/help/6', title: lngStr('help:menuItem.workWithCoach') },
              ],
            },
            {
              title: 'Тренеру',
              itemId: '/help/20',
              //elemBefore: () => <Icon name="category" />,
              subNav: [
                { itemId: '/help/21', title: lngStr('help:menuItem.exerciseCreation') },
                { itemId: '/help/22', title: 'Заявки спортсменов', },
              ],
            },
          ]}
        />
      </>
    );
  }

}

export default WithRouter(HelpMainView);