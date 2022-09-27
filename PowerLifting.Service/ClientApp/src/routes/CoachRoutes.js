import CoachConsoleView from "../components/coaching/CoachConsoleView";
import CoachSelection from "../components/userData/CoachSelection";

const CoachRoutes = [
  { path: '/coachConsole', element: <CoachConsoleView /> },
  { path: '/coachSelection', element: <CoachSelection /> }
];

export default CoachRoutes;