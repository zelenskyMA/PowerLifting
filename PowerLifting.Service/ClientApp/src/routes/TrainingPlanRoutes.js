import PlanCreate from "../components/trainingPlan/edit/PlanCreate";
import PlanDaysCreate from "../components/trainingPlan/edit/PlanDaysCreate";
import PlanDayCreate from "../components/trainingPlan/edit/PlanDayCreate";
import PlanExercisesCreate from "../components/trainingPlan/edit/PlanExercisesCreate";
import PlanExerciseSettingsEdit from "../components/trainingPlan/edit/PlanExerciseSettingsEdit";

import PlansListView from "../components/trainingPlan/view/PlansListView";

const TrainingPlanRoutes = [
  { path: '/createPlanExercises/:id', element: <PlanExercisesCreate /> },
  { path: '/createPlanDays', element: <PlanDaysCreate /> },
  { path: '/createPlanDay/:id', element: <PlanDayCreate /> },
  { path: '/createPlan', element: <PlanCreate /> },
  { path: '/editPlanExerciseSettings/:dayId/:id', element: <PlanExerciseSettingsEdit /> },

  { path: '/plansList', element: <PlansListView /> }
];

export default TrainingPlanRoutes;