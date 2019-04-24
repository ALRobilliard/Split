import React, { Component } from 'react';
import './AddAccount.css';
import { Link, BrowserRouter as Router, Redirect } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { postData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  accountName: string,
  redirect: boolean
}

class AddAccount extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      accountName: '',
      redirect: false
    };
  }

  accountNameOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ accountName: e.target.value });

  postAccount = () => {
    const accountName = this.state.accountName,
      token = 
        this.props.user != null ? 
        this.props.user.token : 
        undefined;
        
    if (accountName != '') {
      postData(`${baseUrl}/api/accounts`, accountName, token)
        .then(res => {
          const state = this.state;
          this.setState({
            accountName: state.accountName,
            redirect: true
          })
        });
    }
  }

  render() {
    const redirect = this.state.redirect;

    if (redirect) {
      return <Redirect to="/accounts" />
    }

    return (
      <div className="addAccount">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Account</h1>
        </div>
        <div className="form">
          <label htmlFor="accountName">Account Name:</label>
          <input 
            id="accountName" 
            type="text"
            onChange={this.accountNameOnChange}
          />
          <div className="form-actions">
            <Link to="/accounts"><button className="button button-close">Cancel</button></Link>
            <a><button className="button button-confirm" onClick={this.postAccount}>Create</button></a>
          </div>
        </div>
      </div>
    )
  }
}

export default AddAccount;