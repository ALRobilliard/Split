import { FETCH_USER } from './types';
import { Dispatch } from 'redux';
import { postData } from '../helpers/utils';
import { apiBaseUrl } from '../private/config';

export function fetchUser() {
  return function(dispatch: Dispatch) {
    const url = apiBaseUrl + '/api/users/authenticate';
    const loginData = {
      Username: 'TestUser',
      Password: 'testuser'
    };

    postData(url, loginData)
      .then(user => dispatch({
        type: FETCH_USER,
        payload: user
      }));
  }
}