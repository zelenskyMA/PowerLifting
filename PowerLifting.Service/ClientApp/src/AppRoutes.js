import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { WorkDayView } from "./components/workDay/WorkDayView";

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
    path: '/workDay',
    element: <WorkDayView />
  }
];

export default AppRoutes;
