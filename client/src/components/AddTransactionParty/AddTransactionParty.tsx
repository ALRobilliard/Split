import React, { Component } from 'react';
import './AddTransactionParty.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class AddTransactionParty extends Component {
  render() {
    return (
      <div className="addTransactionParty">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Transaction Party</h1>
          <Link to="/transactionparties" className="close-wrapper"><button className="button button-close"><i className="fas fa-times-circle"></i></button></Link>
        </div>
      </div>
    )
  }
}

export default AddTransactionParty;