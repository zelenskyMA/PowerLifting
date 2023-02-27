
import HelpMainView from "../components/help/HelpMainView";

const MiscRoutes = [
  { path: '/help', element: <HelpMainView /> },
  { path: '/help/:id', element: <HelpMainView /> },
];

export default MiscRoutes;