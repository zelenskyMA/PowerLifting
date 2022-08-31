import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
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
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/trainingDay',
    element: <TrainingDayView />
  },
  {
    path: '/addTrainingPlan',
    element: <TrainingPlanCreate />
  },
  {
    path: '/exerciseList',
    elemen: <ExerciseList />
  }
];

export default AppRoutes;
