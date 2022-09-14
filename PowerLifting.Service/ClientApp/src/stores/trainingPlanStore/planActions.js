import { GetAsync, PostAsync } from "../../common/ApiActions"

export const RECEIVE_PLAN_ID = "RECEIVE_PLAN_ID";

export async function createTrainingPlan(creationDate, dispatch) {
  const response = await PostAsync("trainingPlan/create", creationDate);
  dispatch({ type: RECEIVE_PLAN_ID, result: response });
}

export async function setTrainingPlan(id, dispatch) {
  dispatch({ type: RECEIVE_PLAN_ID, result: id });
}