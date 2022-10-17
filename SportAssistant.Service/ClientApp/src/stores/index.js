import { combineReducers } from 'redux'
import { planReducer } from './trainingPlanStore/reducers/planReducer'
import { coachReducer } from './coachingStore/reducers/coachReducer'
import { appReducer } from './appStore/reducers/appReducer'

export const rootReducer = combineReducers({
  trainingPlan: planReducer,
  coach: coachReducer,
  app: appReducer
})