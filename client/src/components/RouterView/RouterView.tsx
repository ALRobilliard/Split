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
  refreshUser: Function,
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
        <Route 
          path="/accounts/add"
          render={(props) => <AddAccount {...props} user={this.props.user} />}
        />
        <Route 
          path="/accounts"
          render={(props) => <Accounts {...props} user={this.props.user} />}
        />
        <Route 
          path="/categories/add" 
          render={(props) => <AddCategory {...props} user={this.props.user} />}
        />
        <Route 
          path="/categories" 
          render={(props) => <Categories {...props} user={this.props.user} />}
        />
        <Route path="/settings" component={Settings} />
        <Route 
          path="/signin"
          render={(props) => <SignIn {...props} refreshUser={this.props.refreshUser} />}
        />
        <Route path="/transactions/add" component={AddTransaction} />
        <Route path="/transactions" component={Transactions} />
        <Route 
          path="/transactionparties/add" 
          render={(props) => <AddTransactionParty {...props} user={this.props.user} />}
        />
        <Route 
          path="/transactionparties" 
          render={(props) => <TransactionParties {...props} user={this.props.user} />}
        />
      </Switch>
    );
  }
}

export default RouterView;