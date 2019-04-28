import React, { Component } from 'react';
import './AddTransaction.css';
import { Link, BrowserRouter as Router, Redirect } from 'react-router-dom';

import SearchDropdown from '../SearchDropdown/SearchDropdown';

import { baseUrl } from '../../private/config';
import { postData, getData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  transactionType: number,
  transactionParties: TransactionPartyDto[],
  selectedTransactionPartyId?: string,
  selectedTransactionPartyName?: string,
  categories: CategoryDto[]
  selectedCategoryId?: string,
  selectedCategoryName?: string,
  accounts: AccountDto[],
  selectedAccountInId?: string,
  selectedAccountInName?: string,
  selectedAccountOutId?: string,
  selectedAccountOutName?: string,
  amount: string,
  isShared: number,
  transactionDate?: Date,
  redirect: boolean
}

interface TransactionPost {
  CategoryId?: string,
  TransactionPartyId?: string,
  AccountInId?: string,
  AccountOutId?: string,
  Amount?: number,
  IsShared?: boolean,
  TransactionDate?: Date
}

function partyCompare(a: TransactionPartyDto, b: TransactionPartyDto): number {
  const partyA = a.transactionPartyName.toUpperCase();
  const partyB = b.transactionPartyName.toUpperCase();

  let comparison = 0;
  if (partyA > partyB) {
    comparison = 1;
  } else if (partyA < partyB) {
    comparison = -1;
  }

  return comparison;
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

function accountCompare(a: AccountDto, b: AccountDto): number {
  const accountA = a.accountName.toUpperCase();
  const accountB = b.accountName.toUpperCase();

  let comparison = 0;
  if (accountA > accountB) {
    comparison = 1;
  } else if (accountA < accountB) {
    comparison = -1;
  }

  return comparison;
}

class AddTransaction extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      transactionType: 0,
      transactionParties: [],
      categories: [],
      accounts: [],
      amount: '$0.00',
      isShared: 0,
      redirect: false
    }
  }

  setTransactionPartyLookup = (lookupVal: SearchEntity) => {
    const party = this.state.transactionParties.filter(party => party.transactionPartyId == lookupVal.id),
      categoryId = party != null && party.length > 0 && party[0].defaultCategoryId != null ? 
        party[0].defaultCategoryId : 
        '';
    this.setState({
      selectedTransactionPartyId: lookupVal.id,
      selectedTransactionPartyName: lookupVal.name
    }, () => {this.setDefaults(categoryId)})
  }

  setCategoryLookup = (lookupVal: SearchEntity) => {
    this.setState({
      selectedCategoryId: lookupVal.id,
      selectedCategoryName: lookupVal.name
    })
  }

  setAccountInLookup = (lookupVal: SearchEntity) => {
    this.setState({
      selectedAccountInId: lookupVal.id,
      selectedAccountInName: lookupVal.name
    })
  }

  setAccountOutLookup = (lookupVal: SearchEntity) => {
    this.setState({
      selectedAccountOutId: lookupVal.id,
      selectedAccountOutName: lookupVal.name
    })
  }

  setDefaults = (categoryId: string) => {
    if (categoryId === '') {
      // Do not set Defaults.
      this.setState({
        transactionType: 0,
        selectedCategoryId: undefined,
        selectedCategoryName: undefined
      });
    } else {
      const token = this.props.user != null ? this.props.user.token : '';
      getData(`${baseUrl}/api/categories/${categoryId}`, token)
      .then((res: CategoryDto) => {
        this.setState({
          transactionType: res.categoryType,
          selectedCategoryId: res.categoryId,
          selectedCategoryName: res.categoryName
        })
      })
    }
  }

  transactionTypeOnChange = (e: React.ChangeEvent<HTMLSelectElement>) =>
    this.setState({ transactionType: parseInt(e.target.value) }, this.getCategories)

  amountOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ amount: e.target.value })

  isSharedOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ isShared: parseInt(e.target.value)})

  transactionDateOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ transactionDate: new Date(e.target.value) })

  getTransactionParties = () => {
    const token = this.props.user != null ? this.props.user.token : '';
    getData(`${baseUrl}/api/transactionparties`, token)
      .then((res: TransactionPartyDto[]) => {
        this.setState({
          transactionParties: res.sort(partyCompare)
        })
      })
  }

  getCategories = () => {
    if (this.state.transactionType > -1) {
      const token = this.props.user != null ? this.props.user.token : '';
      getData(`${baseUrl}/api/categories/retrievebytype/${this.state.transactionType}`, token)
      .then((res: CategoryDto[]) => {
        this.setState({
          categories: res.sort(categoryCompare)
        })
      });
    }
  }

  getAccounts = () => {
    const token = this.props.user != null ? this.props.user.token : '';
    getData(`${baseUrl}/api/accounts`, token)
    .then((res: AccountDto[]) => {
      this.setState({
        accounts: res.sort(accountCompare)
      })
    })
  }

  postTransaction = () => {
    const token = this.props.user != null ? this.props.user.token : undefined,
      categoryId = this.state.selectedCategoryId ? this.state.selectedCategoryId : undefined,
      transactionPartyId = this.state.selectedTransactionPartyId ? this.state.selectedTransactionPartyId : undefined,
      accountOutId = this.state.transactionType != 1 && this.state.selectedAccountOutId ? this.state.selectedAccountOutId : undefined,
      accountInId = this.state.transactionType != 0 && this.state.selectedAccountInId ? this.state.selectedAccountInId : undefined,
      amount = this.state.amount && !isNaN(parseFloat(this.state.amount)) ? parseFloat(this.state.amount) : undefined,
      isShared = this.state.isShared ? this.state.isShared == 1 : undefined,
      transactionDate = this.state.transactionDate ? this.state.transactionDate : undefined;

    const transactionObj: TransactionPost = {
      CategoryId: categoryId,
      TransactionPartyId: transactionPartyId,
      AccountOutId: accountOutId,
      AccountInId: accountInId,
      Amount: amount,
      IsShared: isShared,
      TransactionDate: transactionDate
    }

    postData(`${baseUrl}/api/transactions`, transactionObj, token)
    .then(res => {
      this.setState({
        redirect: true
      })
    })
  }

  componentWillMount() {
    this.getTransactionParties();
    this.getAccounts();
  }

  render() {
    const redirect = this.state.redirect;

    if (redirect) {
      return <Redirect to="/transactions" />
    }

    return (
      <div className="addTransaction">
        <div className="headingDiv">
          <h1 className="mainHeading">Add New Transaction</h1>
        </div>
        <div className="form">
          <div className="form-line">
            <SearchDropdown
              labelText='Transaction Party:'
              entityList={this.state.transactionParties.map(transactionParty => {
                return {
                  id: transactionParty.transactionPartyId,
                  name: transactionParty.transactionPartyName
                }
              })} 
              selectedVal={this.state.selectedTransactionPartyName}
              setLookup={this.setTransactionPartyLookup}
            />
          </div>
          <div className="form-line">
            <label htmlFor="transactionType">Transaction Type:</label>
            <select 
              name="transactionType" 
              id="transactionType"
              onChange={this.transactionTypeOnChange}
              value={this.state.transactionType}
            >
              <option value="0">Expense</option>
              <option value="1">Income</option>
              <option value="2">Transfer</option>
            </select>
          </div>
          <div className="form-line">
            <SearchDropdown
              labelText='Category:'
              entityList={this.state.categories.map(category => {
                return {
                  id: category.categoryId,
                  name: category.categoryName
                }
              })} 
              selectedVal={this.state.selectedCategoryName}
              setLookup={this.setCategoryLookup}
            />
          </div>
          { this.state.transactionType === 0 || this.state.transactionType === 2 ?
            <div className="form-line">
              <SearchDropdown
                labelText='Account Out:'
                entityList={this.state.accounts.map(account => {
                  return {
                    id: account.accountId,
                    name: account.accountName
                  }
                })} 
                selectedVal={this.state.selectedAccountOutName}
                setLookup={this.setAccountOutLookup}
              />
            </div> :
            null
          }
          { this.state.transactionType === 1 || this.state.transactionType === 2 ?
            <div className="form-line">
              <SearchDropdown
                labelText='Account In:'
                entityList={this.state.accounts.map(account => {
                  return {
                    id: account.accountId,
                    name: account.accountName
                  }
                })} 
                selectedVal={this.state.selectedAccountInName}
                setLookup={this.setAccountInLookup}
              />
            </div> :
            null
          }
          <div className="form-line">
            <label htmlFor="amount">Amount:</label>
            <input 
              id="amount" 
              type="text"
              onChange={this.amountOnChange}
              value={this.state.amount}
            />
          </div>
          <div className="form-line">
            <label htmlFor="transactionDate">Transaction Date:</label>
            <input 
              type="date" 
              name="transactionDate"
              onChange={this.transactionDateOnChange}
            />
          </div>
          <div className="form-line">
            <label htmlFor="isShared">Is Shared?:</label>
            <input 
              type="radio" 
              name="isShared" 
              value="0"
              checked={this.state.isShared === 0}
              onChange={this.isSharedOnChange}
            />
            <input 
              type="radio" 
              name="isShared" 
              value="1" 
              checked={this.state.isShared === 1}
              onChange={this.isSharedOnChange}
            />
          </div>
          <div className="form-actions">
            <Link to="/transactions"><button className="button button-close">Cancel</button></Link>
            <a><button className="button button-confirm" onClick={this.postTransaction}>Create</button></a>
          </div>
        </div>
      </div>
    )
  }
}

export default AddTransaction;