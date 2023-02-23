import { GetAsync, PostAsync } from "../../common/ApiActions";

export const RECEIVE_USER_INFO = "RECEIVE_USER_INFO";
export const RECEIVE_SETTINGS = "RECEIVE_SETTINGS";
export const CHANGE_MODAL_VISIBILITY = "CHANGE_MODAL_VISIBILITY";

export async function updateUserInfo(userInfo, dispatch) {
  await PostAsync(`/userInfo`, { info: userInfo });
  setUserInfo(dispatch);
}

export async function setUserInfo(dispatch) {
  const response = await GetAsync("/userInfo");
  dispatch({ type: RECEIVE_USER_INFO, result: response });
}

export async function initApp(dispatch) {
  const response = await GetAsync("/appSettings/get");
  dispatch({ type: RECEIVE_SETTINGS, result: response });

  setUserInfo(dispatch);
}

export async function changeModalVisibility(modalInfo, dispatch) {
  dispatch({ type: CHANGE_MODAL_VISIBILITY, result: modalInfo });
}

