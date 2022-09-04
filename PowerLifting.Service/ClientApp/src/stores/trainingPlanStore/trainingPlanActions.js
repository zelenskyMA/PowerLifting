import { Get, PostAsync } from "../../common/ApiActions"

export async function createTrainingPlan(creationDate, dispatch) {
  const response = await PostAsync("trainingPlan/create", creationDate);
  const data = await response.json();

  getTrainingPlan(data.id, dispatch);
}

export function getTrainingPlan(id, dispatch) {
  Get(id, "trainingPlan", dispatch);
}