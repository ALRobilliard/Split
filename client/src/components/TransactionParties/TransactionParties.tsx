import React, { Component } from 'react';
import './TransactionParties.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class TransactionParties extends Component {
  render() {
    return (
      <div className="transactionParties">
        <div className="headingDiv">
          <h1 className="mainHeading">Transaction Parties</h1>
          <Link to="/transactionparties/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Transaction Party</button></Link>
        </div>
      </div>
    )
  }
}

export default TransactionParties;