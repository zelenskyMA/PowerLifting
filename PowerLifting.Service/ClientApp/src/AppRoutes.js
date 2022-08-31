import { Home } from "./components/Home";
import { TrainingDayView } from "./components/trainingDay/TrainingDayView";
import TrainingPlanCreate from "./components/trainingPlan/TrainingPlanCreate";
import { ExerciseList } from "./components/exercise/ExerciseList";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/exercises',
    element: <ExerciseList />
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
