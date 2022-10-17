import { RECEIVE_USER_INFO, RECEIVE_SETTINGS } from "../appActions";

const initialState = {
  settings: Object,
  myInfo: Object,
}

export function appReducer(state = initialState, action) {
  switch (action.type) {
    case RECEIVE_USER_INFO: return { ...state, myInfo: action.result };
    case RECEIVE_SETTINGS: return { ...state, settings: action.result };
    default: return state
  }
}
