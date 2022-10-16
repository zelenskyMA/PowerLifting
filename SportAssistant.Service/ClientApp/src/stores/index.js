import { combineReducers } from 'redux'
import { planReducer } from './trainingPlanStore/reducers/planReducer'
import { coachReducer } from './coachingStore/reducers/coachReducer'

export const rootReducer = combineReducers({
  trainingPlan: planReducer,
  coach: coachReducer,
})