import React, { Component } from 'react';
import './AddCategory.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class AddCategory extends Component {
  render() {
    return (
      <div className="addCategory">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Category</h1>
          <Link to="/categories" className="close-wrapper"><button className="button button-close"><i className="fas fa-times-circle"></i></button></Link>
        </div>
      </div>
    )
  }
}

export default AddCategory;