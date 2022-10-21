import { RECEIVE_USER_INFO, RECEIVE_SETTINGS, CHANGE_MODAL_VISIBILITY } from "../appActions";

const initialState = {
  settings: Object,
  myInfo: Object,
  modalInfo: { isVisible: false, headerText: "", buttons: [], body: () => { return (<p></p>) } },
}

export function appReducer(state = initialState, action) {
  switch (action.type) {
    case RECEIVE_USER_INFO: return { ...state, myInfo: action.result };
    case RECEIVE_SETTINGS: return { ...state, settings: action.result };
    case CHANGE_MODAL_VISIBILITY: return { ...state, modalInfo: action.result };
    default: return state
  }
}
