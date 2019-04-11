import React, { Component } from 'react';
import './SideNavigation.css';
import { Route, NavLink, BrowserRouter as Router } from 'react-router-dom'

class SideNavigation extends Component {
  render() {
    return (
      <div className="sideNavigation">
        <nav>
          <ul>
            <li>
              <NavLink exact to="/"><button><i className="fas fa-home"></i>Home</button></NavLink>
            </li>
            <li>
              <NavLink exact to="/transactions"><button><i className="fas fa-money-check-alt"></i>Transactions</button></NavLink>
            </li>
            <li>
              <NavLink exact to="/accounts"><button><i className="fas fa-piggy-bank"></i>Accounts</button></NavLink>
            </li>
            <li>
              <NavLink exact to="/categories"><button><i className="fas fa-tags"></i>Categories</button></NavLink>
            </li>
            <li>
              <NavLink exact to="/transactionparties"><button><i className="fas fa-building"></i>TransactionParties</button></NavLink>
            </li>
          </ul>
        </nav>
        <div className="footer">
          <span>TODO: footer</span>
        </div>
      </div>
    )
  }
}

export default SideNavigation;