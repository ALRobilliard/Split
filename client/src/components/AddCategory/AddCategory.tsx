import React, { Component } from 'react';
import './AddCategory.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class AddCategory extends Component {
  render() {
    return (
      <div className="addCategory">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Category</h1>
        </div>
        <div className="form">
          <div className="form-line">
            <label htmlFor="categoryName">Category Name:</label>
            <input id="categoryName" type="text"/>
          </div>
          <div className="form-line">
            <label htmlFor="categoryType">Category Type:</label>
            <select name="categoryType" id="categoryType">
              <option value="0">Expense</option>
              <option value="1">Income</option>
              <option value="2">Transfer</option>
            </select>
          </div>
          <div className="form-actions">
            <Link to="/categories"><button className="button button-close">Cancel</button></Link>
            <Link to="/categories"><button className="button button-confirm">Create</button></Link>
          </div>
        </div>
      </div>
    )
  }
}

export default AddCategory;