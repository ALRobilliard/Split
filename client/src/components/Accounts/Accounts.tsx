import React, { Component } from 'react';
import './Accounts.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class Accounts extends Component {
  render() {
    return (
      <div className="accounts">
        <div className="headingDiv">
          <h1 className="mainHeading">Accounts</h1>
          <Link to="/accounts/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Account</button></Link>
        </div>
      </div>
    )
  }
}

export default Accounts;