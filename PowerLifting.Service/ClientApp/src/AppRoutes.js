import { Home } from "./components/Home";
import { TrainingDayView } from "./components/trainingPlan/TrainingDayView";
import TrainingPlanCreate from "./components/trainingPlan/TrainingPlanCreate";
import { ExerciseSelection } from "./components/trainingPlan/ExerciseSelection";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/exercises',
    element: <ExerciseSelection />
  },
  {
    path: '/trainingDay',
    element: <TrainingDayView />
  },
  {
    path: '/addTrainingPlan',
    element: <TrainingPlanCreate />
  }
];

export default AppRoutes;
