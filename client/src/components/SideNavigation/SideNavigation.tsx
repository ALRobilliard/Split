import React, { Component } from 'react';
import './SideNavigation.css';
import { Route, NavLink, BrowserRouter as Router } from 'react-router-dom';

interface IProps {
  user?: UserDto
}

interface IState {}

class SideNavigation extends Component<IProps, IState> {
  state: IState

  constructor(props: IProps) {
    super(props);
    this.state = {}
  }

  render() {
    const msg = this.props.user != null ?
      `Hello, ${this.props.user.firstName}!` :
      'Please sign in.';

    const navList = this.props.user != null ?
      <ul>
        <li>
          <NavLink exact to="/"><button><i className="fas fa-chart-line"></i>Dashboard</button></NavLink>
        </li>
        <li>
          <NavLink to="/accounts"><button><i className="fas fa-piggy-bank"></i>Accounts</button></NavLink>
        </li>
        <li>
          <NavLink to="/categories"><button><i className="fas fa-tags"></i>Categories</button></NavLink>
        </li>
        <li>
          <NavLink to="/transactions"><button><i className="fas fa-money-check-alt"></i>Transactions</button></NavLink>
        </li>
        <li>
          <NavLink to="/transactionparties"><button><i className="fas fa-building"></i>Transaction Parties</button></NavLink>
        </li>
        <li>
          <NavLink to="/settings"><button><i className="fas fa-cog"></i>Settings</button></NavLink>
        </li>
      </ul> :
      <ul>
        <li>
          <NavLink to="/signin"><button><i className="fas fa-user"></i>Sign In</button></NavLink>
        </li>
      </ul>
      
    return (
      <div className="sideNavigation">
        <div className="logoContainer">
          <span>{msg}</span>
        </div>
        <nav>
          {navList}
        </nav>
        <div className="footer">
          <span>TODO: footer</span>
        </div>
      </div>
    )
  }
}

export default SideNavigation;