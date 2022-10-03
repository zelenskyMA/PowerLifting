import { SET_GROUP_USER_ID } from "../coachActions"

const initialState = {
  groupUserId: 0,
}

export function coachReducer(state = initialState, action) {
  switch (action.type) {
    case SET_GROUP_USER_ID:
      return { ...state, groupUserId: action.result };

    default:
      return state
  }
}
