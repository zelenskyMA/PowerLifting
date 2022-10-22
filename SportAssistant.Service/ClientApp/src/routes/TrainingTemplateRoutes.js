import TemplateSetListView from "../components/trainingTemplate/view/TemplateSetListView";
import TemplateSetView from "../components/trainingTemplate/view/TemplateSetView";

const TrainingTemplateRoutes = [
  { path: '/templateSetList', element: <TemplateSetListView /> },
  { path: '/templateSet/:id', element: <TemplateSetView /> },
];

export default TrainingTemplateRoutes;