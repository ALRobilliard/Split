import React, { Component } from 'react';
import './Transactions.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';
import moment from 'moment';
import { getData } from '../../helpers/utils';
import { baseUrl } from '../../private/config';

interface IProps {
  user?: UserDto
}

interface IState {
  transactions: TransactionDto[],
  startDate: Date,
  endDate: Date
}

function transactionCompare(a: TransactionDto, b:TransactionDto) {
  const transactionA = a.transactionDate;
  const transactionB = b.transactionDate;

  let comparison = 0;
  if (transactionA > transactionB) {
    comparison = -1;
  } else if (transactionA < transactionB) {
    comparison = 1;
  }

  return comparison;
}

class Transactions extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      transactions: [],
      startDate: moment(new Date()).add(-1, 'M').toDate(),
      endDate: new Date()
    }
  }

  getTransactions = () => {
    const token = this.props.user != null ? this.props.user.token : '',
      startDate = moment(this.state.startDate).format('YYYY-MM-DD'),
      endDate = moment(this.state.endDate).format('YYYY-MM-DD');

    getData(`${baseUrl}/api/transactions?startDate=${startDate}&endDate=${endDate}`, token)
    .then((res: TransactionDto[]) => {
      this.setState({
        transactions: res.sort(transactionCompare)
      })
    })
  }

  componentWillMount() {
    this.getTransactions();
  }

  startDateOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ startDate: new Date(e.target.value) })
  endDateOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ endDate: new Date(e.target.value) })

  render() {
    return (
      <div className="transactions">
        <div className="headingDiv">
          <h1 className="mainHeading">Transactions</h1>
          <Link to="/transactions/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Transaction</button></Link>
        </div>
        <div className="mainContent">
          <div className="form">
            <div className="form-line">
              <label htmlFor="startDate">Start Date:</label>
              <input 
                type="date" 
                name="startDate" 
                value={moment(this.state.startDate).format('YYYY-MM-DD')}
                onChange={this.startDateOnChange}
              />
              <label htmlFor="endDate">End Date:</label>
              <input 
                type="date" 
                name="endDate"
                value={moment(this.state.startDate).format('YYYY-MM-DD')}
                onChange={this.endDateOnChange}
              />
            </div>
          </div>
          <ul className="entityList">
            {this.state.transactions.map((value, index) => {
              return <li key={value.transactionId}>{moment(value.transactionDate).format('DD MMMM YYYY')} | {value.transactionPartyName} - {value.categoryName} | {'$' + (value.amount != null ? value.amount : 0).toFixed(2)}</li>
            })}
          </ul>
        </div>
      </div>
    )
  }
}

export default Transactions;