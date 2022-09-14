import { RECEIVE_PLAN_ID } from "../planActions"

const initialState = {
  planId: 0,
}

export function planReducer(state = initialState, action) {
  switch (action.type) {   
    case RECEIVE_PLAN_ID:
      return { ...state, planId: action.result };

    default:
      return state
  }
}
