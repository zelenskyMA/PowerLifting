import TrainingPlanRoutes from "./TrainingPlanRoutes";
import UserRoutes from "./UserRoutes";
import AnaliticsRoutes from "./AnaliticsRoutes";
import AdminRoutes from "./AdminRoutes";
import CoachRoutes from "./CoachRoutes";
import ExerciseRoutes from "./ExerciseRoutes";

import Home from "../components/main/Home";

const AppRoutes = [
  { index: true, element: <Home /> },

  ...TrainingPlanRoutes,
  ...UserRoutes,
  ...AnaliticsRoutes,
  ...AdminRoutes,
  ...CoachRoutes,
  ...ExerciseRoutes
];

export default AppRoutes;
