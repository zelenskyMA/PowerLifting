import { Home } from "./components/Home";
import { TrainingDayView } from "./components/trainingPlan/TrainingDayView";
import TrainingPlanCreate from "./components/trainingPlan/TrainingPlanCreate";
import TrainingDaysSetup from "./components/trainingPlan/TrainingDaysSetup";
import PlannedExerciseSetup from "./components/trainingPlan/PlannedExerciseSetup";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/plannedExercises/:id',
    element: <PlannedExerciseSetup />
  },
  {
    path: '/trainingDay',
    element: <TrainingDayView />
  },
  {
    path: '/trainingDaysSetup',
    element: <TrainingDaysSetup />
  },
  {
    path: '/addTrainingPlan',
    element: <TrainingPlanCreate />
  }
];

export default AppRoutes;
