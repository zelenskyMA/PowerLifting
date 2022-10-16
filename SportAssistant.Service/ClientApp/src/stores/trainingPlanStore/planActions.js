import { PostAsync } from "../../common/ApiActions"

export const RECEIVE_PLAN_ID = "RECEIVE_PLAN_ID";

export async function createTrainingPlan(request, dispatch) {
  const response = await PostAsync("trainingPlan/create", request);
  dispatch({ type: RECEIVE_PLAN_ID, result: response });
}

export async function setTrainingPlan(id, dispatch) {
  dispatch({ type: RECEIVE_PLAN_ID, result: id });
}