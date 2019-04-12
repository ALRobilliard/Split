import React, { Component } from 'react';
import './AddAccount.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class AddAccount extends Component {
  render() {
    return (
      <div className="addAccount">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Account</h1>
        </div>
        <div className="form">
          <label htmlFor="accountName">Account Name:</label>
          <input id="accountName" type="text"/>
          <div className="form-actions">
            <Link to="/accounts"><button className="button button-close">Cancel</button></Link>
            <Link to="/accounts"><button className="button button-confirm">Create</button></Link>
          </div>
        </div>
      </div>
    )
  }
}

export default AddAccount;