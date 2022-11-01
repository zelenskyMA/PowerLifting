import CoachConsoleView from "../components/coaching/CoachConsoleView";
import RequestAcceptView from "../components/coaching/RequestAcceptView";
import RequestConsoleView from "../components/coaching/RequestConsoleView";
import GroupView from "../components/coaching/GroupView";
import GroupUserView from "../components/coaching/groupUser/GroupUserView";
import GroupConsoleView from "../components/coaching/GroupConsoleView";
import CoachSelection from "../components/userData/CoachSelection";

const CoachRoutes = [
  { path: '/coachConsole', element: <CoachConsoleView /> },
  { path: '/groupConsole', element: <GroupConsoleView /> },
  { path: '/requestConsole', element: <RequestConsoleView /> },
  { path: '/coachSelection', element: <CoachSelection /> },
  { path: '/trainingGroup/:groupId', element: <GroupView /> },
  { path: '/acceptRequest/:requestId', element: <RequestAcceptView /> },
  { path: '/groupUser/:id', element: <GroupUserView /> }
];

export default CoachRoutes;