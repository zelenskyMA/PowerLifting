import { combineReducers } from 'redux'
import { planReducer } from './trainingPlanStore/reducers/planReducer'

export const rootReducer = combineReducers({
  trainingPlan: planReducer,
})