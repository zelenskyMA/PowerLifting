import CoachConsoleView from "../components/coaching/CoachConsoleView";
import RequestAcceptView from "../components/coaching/RequestAcceptView";
import GroupView from "../components/coaching/GroupView";
import GroupUserView from "../components/coaching/groupUser/GroupUserView";
import CoachSelection from "../components/userData/CoachSelection";

const CoachRoutes = [
  { path: '/coachConsole', element: <CoachConsoleView /> },
  { path: '/coachSelection', element: <CoachSelection /> },
  { path: '/trainingGroup/:groupId', element: <GroupView /> },
  { path: '/acceptRequest/:requestId', element: <RequestAcceptView /> },
  { path: '/groupUser/:id', element: <GroupUserView /> }
];

export default CoachRoutes;