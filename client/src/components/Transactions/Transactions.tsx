import React, { Component } from 'react';
import './Transactions.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class Transactions extends Component {
  render() {
    return (
      <div className="transactions">
        <div className="headingDiv">
          <h1 className="mainHeading">Transactions</h1>
          <Link to="/transactions/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Transaction</button></Link>
        </div>
      </div>
    )
  }
}

export default Transactions;