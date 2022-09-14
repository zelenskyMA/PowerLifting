import PlanCreate from "../components/trainingPlan/PlanCreate";
import PlanDaysCreate from "../components/trainingPlan/PlanDaysCreate";
import PlanDayCreate from "../components/trainingPlan/PlanDayCreate";
import PlanExercisesCreate from "../components/trainingPlan/PlanExercisesCreate";
import PlanExerciseSettingsEdit from "../components/trainingPlan/PlanExerciseSettingsEdit";

import PlansList from "../components/plans/PlansList";

const TrainingPlanRoutes = [
  { path: '/createPlanExercises/:id', element: <PlanExercisesCreate /> },
  { path: '/createPlanDays', element: <PlanDaysCreate /> },
  { path: '/createPlanDay/:id', element: <PlanDayCreate /> },
  { path: '/createPlan', element: <PlanCreate /> },
  { path: '/editPlanExerciseSettings/:dayId/:id', element: <PlanExerciseSettingsEdit /> },

  { path: '/plansList', element: <PlansList /> }
];

export default TrainingPlanRoutes;