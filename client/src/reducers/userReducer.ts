import { FETCH_USER } from '../actions/types';
import { AnyAction } from 'redux';

const initialState = {
  user: {}
}

export default function(state = initialState, action: AnyAction) {
  switch(action.type) {
    case FETCH_USER:
      return {
        ...state,
        user: action.payload
      }
    default:
      return state;
  }
}