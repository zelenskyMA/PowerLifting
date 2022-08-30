import { combineReducers } from 'redux'
import { trainingPlanReducer } from '../stores/trainingPlanStore/reducers/trainingPlanReducer'

export const rootReducer = combineReducers({
  trainingPlan: trainingPlanReducer,
})