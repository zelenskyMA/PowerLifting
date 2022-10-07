import ExercisesView from "../components/exercises/ExercisesView";
import ExerciseEdit from "../components/exercises/ExerciseEdit";

const ExerciseRoutes = [
  { path: '/exercises', element: <ExercisesView /> },
  { path: '/exercises/edit/:id', element: <ExerciseEdit /> },
  { path: '/exercises/add', element: <ExerciseEdit /> },
];

export default ExerciseRoutes;