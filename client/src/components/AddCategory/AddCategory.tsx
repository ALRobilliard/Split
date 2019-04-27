import React, { Component } from 'react';
import './AddCategory.css';
import { Link, BrowserRouter as Router, Redirect } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { postData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  categoryName: string,
  categoryType: number,
  redirect: boolean
}

class AddCategory extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      categoryName: '',
      categoryType: 0,
      redirect: false
    }
  }

  categoryNameOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ categoryName: e.target.value });
  categoryTypeOnChange = (e: React.ChangeEvent<HTMLSelectElement>) => this.setState({ categoryType: parseInt(e.target.value) })

  postCategory = () => {
    const categoryName = this.state.categoryName,
      categoryType = this.state.categoryType,
      token = 
        this.props.user != null ? 
        this.props.user.token : 
        undefined,
      categoryObj = {
        CategoryName: categoryName,
        CategoryType: categoryType
      }
        
    if (categoryName != '') {
      postData(`${baseUrl}/api/categories`, categoryObj, token)
        .then(res => {
          const state = this.state;
          this.setState({
            categoryName: state.categoryName,
            categoryType: state.categoryType,
            redirect: true
          })
        });
    }
  }

  render() {
    const redirect = this.state.redirect;

    if (redirect) {
      return <Redirect to="/categories" />
    }

    return (
      <div className="addCategory">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Category</h1>
        </div>
        <div className="form">
          <div className="form-line">
            <label htmlFor="categoryName">Category Name:</label>
            <input 
              id="categoryName" 
              type="text"
              onChange={this.categoryNameOnChange}
            />
          </div>
          <div className="form-line">
            <label htmlFor="categoryType">Category Type:</label>
            <select name="categoryType" id="categoryType" onChange={this.categoryTypeOnChange}>
              <option value="0">Expense</option>
              <option value="1">Income</option>
              <option value="2">Transfer</option>
            </select>
          </div>
          <div className="form-actions">
            <Link to="/categories"><button className="button button-close">Cancel</button></Link>
            <a><button className="button button-confirm" onClick={this.postCategory}>Create</button></a>
          </div>
        </div>
      </div>
    )
  }
}

export default AddCategory;