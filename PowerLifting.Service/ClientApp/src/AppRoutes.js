import { Home } from "./components/Home";
import PlanCreate from "./components/trainingPlan/PlanCreate";
import PlanDaysCreate from "./components/trainingPlan/PlanDaysCreate";
import PlanDayCreate from "./components/trainingPlan/PlanDayCreate";
import PlanExercisesCreate from "./components/trainingPlan/PlanExercisesCreate";
import PlanExerciseSettingsEdit from "./components/trainingPlan/PlanExerciseSettingsEdit";

const AppRoutes = [
  { index: true, element: <Home /> },

  { path: '/createPlanExercises/:id', element: <PlanExercisesCreate /> },
  { path: '/createPlanDays', element: <PlanDaysCreate /> },
  { path: '/createPlanDay/:id', element: <PlanDayCreate /> },
  { path: '/createPlan', element: <PlanCreate /> },
  { path: '/editPlanExerciseSettings/:dayId/:id', element: <PlanExerciseSettingsEdit /> }
];

export default AppRoutes;
