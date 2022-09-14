import TrainingPlanRoutes from "./TrainingPlanRoutes";
import UserRoutes from "./UserRoutes";

import Home from "../components/Home";

const AppRoutes = [
  { index: true, element: <Home /> },

  ...TrainingPlanRoutes,
  ...UserRoutes
];

export default AppRoutes;
