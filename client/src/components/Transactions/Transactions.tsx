import React, { Component } from 'react';
import './Transactions.css';
import { Link, BrowserRouter as Router } from 'react-router-dom';
import moment from 'moment';
import { getData, deleteData } from '../../helpers/utils';
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
  _isMounted = false;
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
      if (this._isMounted) {
        this.setState({
          transactions: res.sort(transactionCompare)
        });
      }
    })
  }

  deleteTransaction = (transactionId: string, transactionDate: string, transactionAmount: string) => {
    const deleteConfirmed = window.confirm(`Are you sure you want to delete transaction on '${transactionDate}', for '$${transactionAmount}'?`);
    if (deleteConfirmed) {
      const token = this.props.user != null ? this.props.user.token : '';
      deleteData(`${baseUrl}/api/transactions/${transactionId}`, token)
      .then((res) => {
        this.getTransactions();
      });
    }
  }

  componentWillMount() {
    this._isMounted = true;
    this.getTransactions();
  }

  componentWillUnmount() {
    this._isMounted = false;
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
            <div className="form-line inline">
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
          <div className="dashboardList">
            <h2 className="listHeading">My Transactions</h2>
            <div className="separator"></div>
            <table className="dataTable">
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Party</th>
                  <th>Category</th>
                  <th>Is Shared?</th>
                  <th className="money">Amount ($)</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {this.state.transactions.map((value, index) => {
                  let classes: string[] = ['money'];
                  if (value.accountOutId && !value.accountInId) {
                    classes.push('expense');
                  } else if (!value.accountOutId && value.accountInId) {
                    classes.push('income');
                  } else {
                    classes.push('transfer');
                  }

                  const isShared = value.isShared ? <i className="fas fa-check" style={{color: "#4caf50"}}></i> : <i className="fas fa-times" style={{color: "#f44336"}}></i>;

                  return (
                    <tr key={value.transactionId}>
                      <td>{moment(value.transactionDate).format('DD MMMM')}</td>
                      <td>{value.transactionPartyName}</td>
                      <td>{value.categoryName}</td>
                      <td>{isShared}</td>
                      <td className={classes.join(" ")}>{(value.amount != null ? value.amount : 0).toFixed(2)}</td>
                      <td><button className="delete" onClick={() => this.deleteTransaction(value.transactionId, moment(value.transactionDate).format('DD MMMM'), (value.amount != null ? value.amount : 0).toFixed(2))}><i className="fas fa-trash"></i></button></td>
                    </tr>
                )})}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    )
  }
}

export default Transactions;