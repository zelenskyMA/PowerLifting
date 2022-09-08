import { REQUEST_TRAININGPLAN, RECEIVE_PLAN_ID } from "../planActions"

const initialState = {
  planId: 0,
  isFetching: false,
}

export function planReducer(state = initialState, action) {
  switch (action.type) {
    case REQUEST_TRAININGPLAN:
      return { ...state, isFetching: true }

    case RECEIVE_PLAN_ID:
      return { ...state, planId: action.result };

    default:
      return state
  }
}
