import TemplatePlanEdit from "../components/trainingTemplate/edit/TemplatePlanEdit";
import TemplateDayEdit from "../components/trainingTemplate/edit/TemplateDayEdit";
import TemplateExercisesEdit from "../components/trainingTemplate/edit/TemplateExercisesEdit";
import TemplateExerciseSettingsEdit from "../components/trainingTemplate/edit/TemplateExerciseSettingsEdit";

import TemplateSetListView from "../components/trainingTemplate/view/TemplateSetListView";
import TemplateSetView from "../components/trainingTemplate/view/TemplateSetView";

const TrainingTemplateRoutes = [
  { path: '/editTemplatePlan/:id', element: <TemplatePlanEdit /> },
  { path: '/editTemplateDay/:id', element: <TemplateDayEdit /> },
  { path: '/editTemplateExercises/:dayId', element: <TemplateExercisesEdit /> },
  { path: '/editTemplateExerciseSettings/:id', element: <TemplateExerciseSettingsEdit /> },

  { path: '/templateSetList', element: <TemplateSetListView /> },
  { path: '/templateSet/:id', element: <TemplateSetView /> },
];

export default TrainingTemplateRoutes;