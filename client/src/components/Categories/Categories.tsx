import React, { Component } from 'react';
import './Categories.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { getData } from '../../helpers/utils';
import { stat } from 'fs';

interface IProps {
  user?: UserDto
}

interface IState {
  categories: CategoryDto[],
  categoryType: number
}

function categoryCompare(a: CategoryDto, b: CategoryDto): number {
  const categoryA = a.categoryName.toUpperCase();
  const categoryB = b.categoryName.toUpperCase();

  let comparison = 0;
  if (categoryA > categoryB) {
    comparison = 1;
  } else if (categoryA < categoryB) {
    comparison = -1;
  }

  return comparison;
}

class Categories extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      categories: [],
      categoryType: 0
    }
  }

  categoryTypeOnChange = (e: React.ChangeEvent<HTMLSelectElement>) => { 
    this.setState(
      {categoryType: parseInt(e.target.value)}, 
      this.retrieveCategories
    )
  }

  retrieveCategories = () => {
    const token = this.props.user != null ? this.props.user.token : '';
    getData(`${baseUrl}/api/categories/retrievebytype/${this.state.categoryType}`, token)
    .then((res: CategoryDto[]) => {
      this.setState({
        categories: res.sort(categoryCompare)
      })
    });
  }

  componentDidMount() {
    this.retrieveCategories();
  }

  render() {
    return (
      <div className="categories">
        <div className="headingDiv">
          <h1 className="mainHeading">Categories</h1>
          <Link to="/categories/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Category</button></Link>
        </div>
        <div className="mainContent">
          <label htmlFor="categoryType">Category Type:</label>
          <select name="categoryType" id="categoryType" onChange={this.categoryTypeOnChange}>
            <option value="0">Expense</option>
            <option value="1">Income</option>
            <option value="2">Transfer</option>
          </select>
          <ul className="entityList">
            {this.state.categories.map((value, index) => {
              return <li key={value.categoryId}>{value.categoryName}</li>
            })}
          </ul>
        </div>
      </div>
    )
  }
}

export default Categories;