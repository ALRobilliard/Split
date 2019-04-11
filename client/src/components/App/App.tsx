import React, { Component } from 'react';
import { render } from 'react-dom';
import { Route, Switch, BrowserRouter as Router } from 'react-router-dom';
import './App.css';
import SideNavigation from '../SideNavigation/SideNavigation';
import Accounts from '../Accounts/Accounts';
import Categories from '../Categories/Categories';
import Settings from '../Settings/Settings';
import Transactions from '../Transactions/Transactions';
import TransactionParties from '../TransactionParties/TransactionParties';

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
              <Route exact path="/" component={() => (<h1 className="mainHeading">Dashboard</h1>)} />
              <Route path="/accounts" component={Accounts} />
              <Route path="/categories" component={Categories} />
              <Route path="/settings" component={Settings} />
              <Route path="/transactions" component={Transactions} />              
              <Route path="/transactionParties" component={TransactionParties} />              
            </Switch>
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
