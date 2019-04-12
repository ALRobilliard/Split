import React, { Component } from 'react';
import './Categories.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

class Categories extends Component {
  render() {
    return (
      <div className="categories">
        <div className="headingDiv">
          <h1 className="mainHeading">Categories</h1>
          <Link to="/categories/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Category</button></Link>
        </div>
      </div>
    )
  }
}

export default Categories;