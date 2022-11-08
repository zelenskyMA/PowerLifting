import PlanCreate from "../components/trainingPlan/edit/PlanCreate";
import PlanDaysEdit from "../components/trainingPlan/edit/PlanDaysEdit";
import PlanDayEdit from "../components/trainingPlan/edit/PlanDayEdit";
import PlanExercisesEdit from "../components/trainingPlan/edit/PlanExercisesEdit";
import PlanExerciseSettingsEdit from "../components/trainingPlan/edit/PlanExerciseSettingsEdit";

import PlansListView from "../components/trainingPlan/view/PlansListView";

const TrainingPlanRoutes = [
  { path: '/createPlan/:groupUserId', element: <PlanCreate /> },  

  { path: '/editPlanDays/:planId', element: <PlanDaysEdit /> },
  { path: '/editPlanDay/:planId/:id', element: <PlanDayEdit /> },
  { path: '/editPlanExercises/:planId/:id', element: <PlanExercisesEdit /> },
  { path: '/editPlanExerciseSettings/:planId/:id', element: <PlanExerciseSettingsEdit /> },

  { path: '/plansList', element: <PlansListView /> }
];

export default TrainingPlanRoutes;