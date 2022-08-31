
const REQUEST_TRAININGPLAN = "REQUEST_TRAININGPLAN";
const RECEIVE_TRAININGPLAN = "RECEIVE_TRAININGPLAN";

const initialState = {
  planId: 0,
  isFetching: false,
}

export function trainingPlanReducer(state = initialState, action) {
  switch (action.type) {
    case REQUEST_TRAININGPLAN:
      return { ...state, id: action.payload, isFetching: true }

    case RECEIVE_TRAININGPLAN:
      return { ...state, trainingPlan: action.payload, isFetching: false }

    default:
      return state
  }
}
