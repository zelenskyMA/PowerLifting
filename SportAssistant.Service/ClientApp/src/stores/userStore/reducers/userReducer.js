import { RECEIVE_USER_INFO } from "../userActions";

const initialState = {
  info: Object,
}

export function userReducer(state = initialState, action) {
  switch (action.type) {
    case RECEIVE_USER_INFO:
      return { ...state, info: action.result };

    default:
      return state
  }
}
