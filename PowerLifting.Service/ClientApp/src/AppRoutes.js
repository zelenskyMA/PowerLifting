import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { TrainingDayView } from "./components/trainingDay/TrainingDayView";
import TrainingPlanCreate from "./components/trainingPlan/TrainingPlanCreate";

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
  }
];

export default AppRoutes;
