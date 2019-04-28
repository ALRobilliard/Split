import React, { Component } from 'react';
import './Accounts.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { getData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  accounts: AccountDto[]
}

class Accounts extends Component<IProps, IState> {
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
      this.setState({
        accounts: res
      })
    });
  }

  componentDidMount() {
    this.retrieveAccounts();
  }
  
  render() {
    return (
      <div className="accounts">
        <div className="headingDiv">
          <h1 className="mainHeading">Accounts</h1>
          <Link to="/accounts/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Account</button></Link>
        </div>
        <div className="mainContent">
          <ul className="entityList">
            {this.state.accounts.map((value, index) => {
              return <li key={value.accountId}>{value.accountName} | {value.accountType == 0 ? 'Debit' : 'Credit'} | {value.balance ? '$' + value.balance.toFixed(2) : null} | {value.limit ? '$' + value.limit.toFixed(2) : null}</li>
            })}
          </ul>
        </div>
      </div>
    )
  }
}

export default Accounts;