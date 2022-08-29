import { Get, Update } from "../../common/ApiActions"

export function createTrainingPlan(plan, dispatch) {
  Update("trainingPlan", plan)
    .then(data => GetTrainingPlan(data.id, dispatch));
}

export function getTrainingPlan(id, dispatch) {
  Get(id, "trainingPlan", dispatch);
}