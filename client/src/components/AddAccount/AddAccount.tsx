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
  accountType: number,
  balance?: number,
  limit?: number,
  redirect: boolean
}

interface AccountPost {
  accountName: string,
  accountType: number,
  balance?: number,
  limit?: number
}

class AddAccount extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      accountName: '',
      accountType: 0,
      redirect: false
    };
  }

  accountNameOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ accountName: e.target.value });
  accountTypeOnChange = (e: React.ChangeEvent<HTMLSelectElement>) => this.setState({ accountType: parseInt(e.target.value) });
  balanceOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ balance: parseFloat(e.target.value) });
  limitOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ limit: parseFloat(e.target.value) });

  postAccount = () => {
    const token = 
        this.props.user != null ? 
        this.props.user.token : 
        undefined,
      accountObj: AccountPost = {
        accountName: this.state.accountName,
        accountType: this.state.accountType,
        balance: this.state.balance ? this.state.balance : undefined,
        limit: this.state.balance && this.state.accountType == 1 ? this.state.limit : undefined
      };

    postData(`${baseUrl}/api/accounts`, accountObj, token)
      .then(res => {
        const state = this.state;
        this.setState({
          accountName: state.accountName,
          redirect: true
        })
      });
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
          <div className="account-line">
            <label htmlFor="accountName">Account Name:</label>
            <input 
              id="accountName" 
              type="text"
              onChange={this.accountNameOnChange}
            />
          </div>
          <div className="account-line">
            <label htmlFor="accountType">Account Type:</label>
            <select name="accountType" onChange={this.accountTypeOnChange}>
              <option value="0">Debit</option>
              <option value="1">Credit</option>
            </select>
          </div>
          <div className="account-line">
            <label htmlFor="balance">Balance:</label>
            <input type="text" name="balance" onChange={this.balanceOnChange}/>
          </div>
          {this.state.accountType == 1 ?
          <div className="account-line">
            <label htmlFor="limit">Credit Limit:</label>
            <input type="text" name="limit" onChange={this.limitOnChange}/>
          </div> : null
          }
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