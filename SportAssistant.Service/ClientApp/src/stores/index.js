import { combineReducers } from 'redux'
import { appReducer } from './appStore/reducers/appReducer'

export const rootReducer = combineReducers({
  app: appReducer
})