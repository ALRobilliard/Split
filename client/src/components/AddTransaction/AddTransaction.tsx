import React, { Component } from 'react';
import './AddTransaction.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class AddTransaction extends Component {
  render() {
    return (
      <div className="addTransaction">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Transaction</h1>
          <Link to="/transactions" className="close-wrapper"><button className="button button-close"><i className="fas fa-times-circle"></i></button></Link>
        </div>
      </div>
    )
  }
}

export default AddTransaction;