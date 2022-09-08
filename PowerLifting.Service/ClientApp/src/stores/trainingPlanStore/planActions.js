import { GetAsync, PostAsync } from "../../common/ApiActions"

export const REQUEST_TRAININGPLAN = "REQUEST_TRAININGPLAN";
export const RECEIVE_TRAININGPLAN = "RECEIVE_TRAININGPLAN";
export const RECEIVE_PLAN_ID = "RECEIVE_PLAN_ID";

export async function createTrainingPlan(creationDate, dispatch) {
  const response = await PostAsync("trainingPlan/create", creationDate);
  dispatch({ type: RECEIVE_PLAN_ID, result: response });
}

export async function getTrainingPlan(id, dispatch) {
  dispatch({ type: REQUEST_TRAININGPLAN });

  const data = await GetAsync(`trainingPlan/get?id=${id}`);

  dispatch({ type: RECEIVE_TRAININGPLAN, result: data });
}