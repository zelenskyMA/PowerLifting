import TrainingPlanRoutes from "./TrainingPlanRoutes";
import TrainingTemplateRoutes from "./TrainingTemplateRoutes";
import UserRoutes from "./UserRoutes";
import AnaliticsRoutes from "./AnaliticsRoutes";
import AdminRoutes from "./AdminRoutes";
import CoachRoutes from "./CoachRoutes";
import ExerciseRoutes from "./ExerciseRoutes";
import MiscRoutes from "./MiscRoutes";

import Home from "../components/main/Home";

const AppRoutes = [
  { index: true, element: <Home /> },

  ...TrainingPlanRoutes,
  ...TrainingTemplateRoutes,
  ...UserRoutes,
  ...AnaliticsRoutes,
  ...AdminRoutes,
  ...CoachRoutes,
  ...ExerciseRoutes,
  ...MiscRoutes
];

export default AppRoutes;
