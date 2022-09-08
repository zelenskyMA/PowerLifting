import { Home } from "./components/Home";
import { TrainingDayView } from "./components/trainingPlan/TrainingDayView";
import PlanCreate from "./components/trainingPlan/PlanCreate";
import PlanDaysCreate from "./components/trainingPlan/PlanDaysCreate";
import PlanExerciseCreate from "./components/trainingPlan/PlanExerciseCreate";

const AppRoutes = [
  { index: true, element: <Home /> },
  {
    path: '/trainingDay',
    element: <TrainingDayView />
  },
  { path: '/createPlanExercises/:id', element: <PlanExerciseCreate /> },
  { path: '/createPlanDays', element: <PlanDaysCreate /> },
  { path: '/createPlan', element: <PlanCreate /> }
];

export default AppRoutes;
