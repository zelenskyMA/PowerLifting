import { combineReducers } from 'redux'
import { planReducer } from './trainingPlanStore/reducers/planReducer'
import { coachReducer } from './coachingStore/reducers/coachReducer'
import { userReducer } from './userStore/reducers/userReducer'

export const rootReducer = combineReducers({
  trainingPlan: planReducer,
  coach: coachReducer,
  currentUser: userReducer
})