import React, { Component } from 'react';
import './AddTransactionParty.css';
import { Link, BrowserRouter as Router, Redirect } from 'react-router-dom';

import SearchDropdown from '../SearchDropdown/SearchDropdown';

import { baseUrl } from '../../private/config';
import { postData, getData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  partyName: string,
  defaultType: number,
  availableCategories: CategoryDto[],
  selectedCategoryId?: string,
  selectedCategoryName?: string,
  redirect: boolean,
}

interface TransactionPartyPost {
  TransactionPartyName: string,
  DefaultCategoryId?: string
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

class AddTransactionParty extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      partyName: '',
      defaultType: -1,
      availableCategories: [],
      selectedCategoryId: undefined,
      selectedCategoryName: undefined,
      redirect: false
    };
  }

  setLookup = (lookupVal: SearchEntity) => {
    this.setState({
      selectedCategoryId: lookupVal.id,
      selectedCategoryName: lookupVal.name
    })
  }

  partyNameOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ partyName: e.target.value });
  defaultTypeOnChange = (e: React.ChangeEvent<HTMLSelectElement>) =>
    this.setState({ defaultType: parseInt(e.target.value) }, this.getCategories)

  getCategories = () => {
    if (this.state.defaultType > -1) {
      const token = this.props.user != null ? this.props.user.token : '';
      getData(`${baseUrl}/api/categories/retrievebytype/${this.state.defaultType}`, token)
      .then((res: CategoryDto[]) => {
        this.setState({
          availableCategories: res.sort(categoryCompare)
        })
      });
    }
  }

  postParty = () => {
    const partyName = this.state.partyName,
      token = 
        this.props.user != null ?
        this.props.user.token :
        undefined,
      transactionPartyObj: TransactionPartyPost = {
        TransactionPartyName: partyName,
        DefaultCategoryId: this.state.selectedCategoryId ? this.state.selectedCategoryId : undefined
      }

    if (partyName != '') {
      postData(`${baseUrl}/api/transactionparties`, transactionPartyObj, token)
        .then(res => {
          const state = this.state;
          this.setState({
            partyName: state.partyName,
            redirect: true
          })
        });
    }
  }

  render() {
    const redirect = this.state.redirect;
    const validForm = this.state.partyName != '';

    if (redirect) {
      return <Redirect to="/transactionparties" />
    }

    return (
      <div className="addTransactionParty">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Transaction Party</h1>
        </div>
        <div className="form">
          <div className="form-line">
            <label htmlFor="partyName">Transaction Party Name:</label>
            <input 
              id="partyName" 
              type="text"
              onChange={this.partyNameOnChange}
            />
          </div>
          <div className="form-line">
            <label htmlFor="defaultType">Default Transaction Type:</label>
            <select name="defaultType" id="defaultType" onChange={this.defaultTypeOnChange}>
              <option value="-1">Don't Specify</option>
              <option value="0">Expense</option>
              <option value="1">Income</option>
              <option value="2">Transfer</option>
            </select>
          </div>
          {this.state.defaultType > -1 ? <div className="form-line">
            <SearchDropdown 
              labelText='Default Category:'
              entityList={this.state.availableCategories.map(category => {
                return {
                  id: category.categoryId,
                  name: category.categoryName
                }
              })} 
              selectedVal={this.state.selectedCategoryName}
              setLookup={this.setLookup}
            />
          </div>: null}
          <div className="form-actions">
            <Link to="/transactionparties"><button className="button button-close">Cancel</button></Link>
            <a><button className="button button-confirm" onClick={this.postParty} disabled={!validForm}>Create</button></a>
          </div>
        </div>
      </div>
    )
  }
}

export default AddTransactionParty;