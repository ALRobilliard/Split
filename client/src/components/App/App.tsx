import React, { Component } from 'react';
import { render } from 'react-dom';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';
import './App.css';
import SideNavigation from '../SideNavigation/SideNavigation';
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

class App extends Component {
  render() {
    return (
      <div className="App">
        <Router>
          <div className="sidebar">
            <SideNavigation />
          </div>
          <div className="main">
            <Switch>
              <Route exact path="/" component={Dashboard} />
              <Route path="/accounts/add" component={AddAccount} />
              <Route path="/accounts" component={Accounts} />
              <Route path="/categories/add" component={AddCategory} />
              <Route path="/categories" component={Categories} />
              <Route path="/settings" component={Settings} />
              <Route path="/signin" component={SignIn} />
              <Route path="/transactions/add" component={AddTransaction} />
              <Route path="/transactions" component={Transactions} />
              <Route path="/transactionparties/add" component={AddTransactionParty} />
              <Route path="/transactionparties" component={TransactionParties} />
            </Switch>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
