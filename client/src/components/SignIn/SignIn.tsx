import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';

import './SignIn.css';
import { baseUrl } from '../../private/config';
import { postData } from '../../helpers/utils';

interface IProps {
  refreshUser: Function
}

interface IState {
  username: string,
  password: string,
  redirect: boolean
}

class SignIn extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      username: '',
      password: '',
      redirect: false
    };
  }

  authUser = () => {
    postData(`${baseUrl}/api/users/authenticate`, {
      Username: this.state.username,
      Password: this.state.password
    }).then(res => {
      this.setSessionStorage(res);
      this.props.refreshUser();

      const state = this.state;
      this.setState({
        username: state.username,
        password: state.password,
        redirect: true
      })
    })
  }

  setSessionStorage = (user: UserDto) => {
    sessionStorage.setItem('userId', user.id);
    sessionStorage.setItem('userFirstName', user.firstName);
    sessionStorage.setItem('userLastName', user.lastName);
    sessionStorage.setItem('userUsername', user.username);
    sessionStorage.setItem('userToken', user.token);
  }

  userOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ username: e.target.value });
  passwordOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ password: e.target.value });

  render() {
    const redirect = this.state.redirect;

    if (redirect) {
      return <Redirect to="/" />
    }

    return (
      <div className="signIn">
        <div className="headingDiv">
          <h1 className="mainHeading">Sign In</h1>
        </div>
        <div className="form">
          <div className="form-line">
            <label htmlFor="username">Username:</label>
            <input id="username" type="text" name="username" onChange={this.userOnChange} />
          </div>
          <div className="form-line">
            <label htmlFor="password">Password:</label>
            <input id="password" type="password" name="password" onChange={this.passwordOnChange} />
          </div>
          <div className="form-actions">
            <button className="button button-confirm" onClick={this.authUser} >Sign In</button>
          </div>
        </div>
      </div>
    )
  }
}

export default SignIn;