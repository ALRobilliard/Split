import React, { Component } from 'react';
import './AddAccount.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class AddAccount extends Component {
  render() {
    return (
      <div className="addAccount">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Account</h1>
          <Link to="/accounts" className="close-wrapper"><button className="button button-close"><i className="fas fa-times-circle"></i></button></Link>
        </div>
      </div>
    )
  }
}

export default AddAccount;