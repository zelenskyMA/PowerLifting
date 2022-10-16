export const SET_GROUP_USER_ID = "SET_GROUP_USER_ID";

export async function setGroupUserId(id, dispatch) {
  dispatch({ type: SET_GROUP_USER_ID, result: id });
}