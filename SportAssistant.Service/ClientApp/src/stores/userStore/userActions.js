import { GetAsync, PostAsync } from "../../common/ApiActions";

export const RECEIVE_USER_INFO = "RECEIVE_USER_INFO";

export async function updateUserInfo(userInfo, dispatch) {
  await PostAsync(`/userInfo/update`, userInfo);
  setUserInfo(dispatch);
}

export async function setUserInfo(dispatch) {
  const response = await GetAsync("/userInfo/get");
  dispatch({ type: RECEIVE_USER_INFO, result: response });
}