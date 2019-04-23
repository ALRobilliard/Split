import React, { Component } from 'react';
import { Switch, Route } from "react-router-dom";

import Accounts from '../Accounts/Accounts';
import Categories from '../Categories/Categories';
import Settings from '../Settings/Settings';
import SignIn from '../SignIn/SignIn';
import Transactions from '../Transactions/Transactions';
import TransactionParties from '../TransactionParties/TransactionParties';
import AddCategory from '../AddCategory/AddCategory';
import AddAccount from '../AddAccount/AddAccount';
import Dashboard from '../Dashboard/Dashboard';
import AddTransaction from '../AddTransaction/AddTransaction';
import AddTransactionParty from '../AddTransactionParty/AddTransactionParty';

interface IProps {
  setUser: Function,
  user?: UserDto
}

interface IState {}

class RouterView extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {}
  }

  render() {
    return (
      <Switch>
        <Route exact path="/" component={Dashboard} />
        <Route path="/accounts/add" component={AddAccount} />
        <Route path="/accounts" component={Accounts} />
        <Route path="/categories/add" component={AddCategory} />
        <Route path="/categories" component={Categories} />
        <Route path="/settings" component={Settings} />
        <Route 
          path="/signin"
          render={(props) => <SignIn {...props} setUser={this.props.setUser} />}
        />
        <Route path="/transactions/add" component={AddTransaction} />
        <Route path="/transactions" component={Transactions} />
        <Route path="/transactionparties/add" component={AddTransactionParty} />
        <Route path="/transactionparties" component={TransactionParties} />
      </Switch>
    );
  }
}

export default RouterView;