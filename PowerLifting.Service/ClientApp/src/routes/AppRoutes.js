import TrainingPlanRoutes from "./TrainingPlanRoutes";
import UserRoutes from "./UserRoutes";
import AnaliticsRoutes from "./AnaliticsRoutes";
import AdminRoutes from "./AdminRoutes";

import Home from "../components/Home";

const AppRoutes = [
  { index: true, element: <Home /> },

  ...TrainingPlanRoutes,
  ...UserRoutes,
  ...AnaliticsRoutes,
  ...AdminRoutes
];

export default AppRoutes;
