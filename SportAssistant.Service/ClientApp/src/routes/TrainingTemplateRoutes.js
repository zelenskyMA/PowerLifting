import TemplatePlanEdit from "../components/trainingTemplate/edit/TemplatePlanEdit";
import TemplateDayEdit from "../components/trainingTemplate/edit/TemplateDayEdit";
import TemplateExercisesEdit from "../components/trainingTemplate/edit/TemplateExercisesEdit";
import TemplateExerciseSettingsEdit from "../components/trainingTemplate/edit/TemplateExerciseSettingsEdit";
import TemplateOfpExerciseEdit from "../components/trainingTemplate/edit/TemplateOfpExerciseEdit";

import TemplateSetListView from "../components/trainingTemplate/view/TemplateSetListView";
import TemplateSetView from "../components/trainingTemplate/view/TemplateSetView";
import TemplateSetAssignView from "../components/trainingTemplate/view/TemplateSetAssignView";

const TrainingTemplateRoutes = [
  { path: '/editTemplatePlan/:id', element: <TemplatePlanEdit /> },
  { path: '/editTemplateDay/:templateId/:id', element: <TemplateDayEdit /> },
  { path: '/editTemplateExercises/:templateId/:dayId', element: <TemplateExercisesEdit /> },

  { path: '/editTemplateExerciseSettings/:templateId/:id', element: <TemplateExerciseSettingsEdit /> },
  { path: '/editTemplateOfpExercise/:templateId/:id', element: <TemplateOfpExerciseEdit /> },  

  { path: '/templateSetList', element: <TemplateSetListView /> },
  { path: '/templateSet/:id', element: <TemplateSetView /> },
  { path: '/assignTemplateSet/:groupId', element: <TemplateSetAssignView /> },  
];

export default TrainingTemplateRoutes;