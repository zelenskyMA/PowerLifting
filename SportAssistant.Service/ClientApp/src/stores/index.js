import { combineReducers } from 'redux'
import { coachReducer } from './coachingStore/reducers/coachReducer'
import { appReducer } from './appStore/reducers/appReducer'

export const rootReducer = combineReducers({
  coach: coachReducer,
  app: appReducer
})