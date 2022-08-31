import { Get, Create } from "../../common/ApiActions"

export function createTrainingPlan(creationDate, dispatch) {
  Create("trainingPlan", creationDate)
    .then(data => getTrainingPlan(data.id, dispatch));
}

export function getTrainingPlan(id, dispatch) {
  Get(id, "trainingPlan", dispatch);
}