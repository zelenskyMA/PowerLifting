import PlanCreate from "../components/trainingPlan/edit/PlanCreate";
import PlanDaysEdit from "../components/trainingPlan/edit/PlanDaysEdit";
import PlanDayEdit from "../components/trainingPlan/edit/PlanDayEdit";
import PlanDayMove from "../components/trainingPlan/edit/PlanDayMove";
import PlanExercisesEdit from "../components/trainingPlan/edit/PlanExercisesEdit";
import PlanExerciseSettingsEdit from "../components/trainingPlan/edit/PlanExerciseSettingsEdit";
import PlanOfpExerciseEdit from "../components/trainingPlan/edit/PlanOfpExerciseEdit";

import PlansListView from "../components/trainingPlan/view/PlansListView";

const TrainingPlanRoutes = [
  { path: '/createPlan/:groupUserId', element: <PlanCreate /> },

  { path: '/editPlanDays/:planId', element: <PlanDaysEdit /> },
  { path: '/editPlanDay/:planId/:id', element: <PlanDayEdit /> },
  { path: '/movePlanDay/:planId/:id', element: <PlanDayMove /> },

  { path: '/editPlanExercises/:planId/:id', element: <PlanExercisesEdit /> },
  { path: '/editPlanExerciseSettings/:planId/:id', element: <PlanExerciseSettingsEdit /> },
  { path: '/editPlanOfpExercise/:planId/:id', element: <PlanOfpExerciseEdit /> },

  { path: '/plansList', element: <PlansListView /> }
];

export default TrainingPlanRoutes;