import React, { Component } from 'react';
import './Accounts.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { getData, deleteData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  accounts: AccountDto[]
}

class Accounts extends Component<IProps, IState> {
  _isMounted = false;
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      accounts: []
    }
  }

  retrieveAccounts = () => {
    const token = this.props.user != null ? this.props.user.token : '';
    getData(`${baseUrl}/api/accounts`, token)
    .then((res: AccountDto[]) => {
      if (this._isMounted) {
        this.setState({
          accounts: res
        })
      }
    });
  }

  deleteAccount = (accountId: string, accountName: string) => {
    const deleteConfirmed = window.confirm(`Are you sure you want to delete account '${accountName}'`);
    if (deleteConfirmed) {
      const token = this.props.user != null ? this.props.user.token : '';
      deleteData(`${baseUrl}/api/accounts/${accountId}`, token)
      .then((res) => {
        this.retrieveAccounts();
      });
    }
  }

  componentWillMount() {
    this._isMounted = true;
    this.retrieveAccounts();
  }

  componentWillUnmount() {
    this._isMounted = false;
  }
  
  render() {
    return (
      <div className="accounts">
        <div className="headingDiv">
          <h1 className="mainHeading">Accounts</h1>
          <Link to="/accounts/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Account</button></Link>
        </div>
        <div className="mainContent">
          <div className="dashboardList">
            <h2 className="listHeading">My Accounts</h2>
            <div className="separator"></div>
            <table className="dataTable">
              <thead>
                <tr>
                  <th>Account Name</th>
                  <th>Account Type</th>
                  <th>Limit</th>
                  <th>Balance</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
              {this.state.accounts.map((value, index) => {
              return (
                <tr key={value.accountId}>
                  <td>{value.accountName}</td>
                  <td>{value.accountType == 0 ? 'Debit' : 'Credit'}</td>
                  <td>{value.limit ? '$' + value.limit.toFixed(2) : '-'}</td>
                  <td>{value.balance ? '$' + value.balance.toFixed(2) : '-'}</td>
                  <td><button className="delete" onClick={() => this.deleteAccount(value.accountId, value.accountName)}><i className="fas fa-trash"></i></button></td>
                </tr>
              )})}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    )
  }
}

export default Accounts;