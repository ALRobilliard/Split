import React, { Component } from 'react';
import './TransactionParties.css';
import { Link } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { getData, deleteData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  transactionParties: TransactionPartyDto[]
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

class TransactionParties extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      transactionParties: []
    }
  }

  retrieveTransactionParties = () => {
    const token = this.props.user != null ? this.props.user.token : '';
    getData(`${baseUrl}/api/transactionparties`, token)
      .then((res: TransactionPartyDto[]) => {
        this.setState({
          transactionParties: res.sort(partyCompare)
        })
      })
  }

  deleteTransactionParty = (transactionPartyId: string, transactionPartyName: string) => {
    const deleteConfirmed = window.confirm(`Are you sure you want to delete Transaction Party '${transactionPartyName}'`);
    if (deleteConfirmed) {
      const token = this.props.user != null ? this.props.user.token : '';
      deleteData(`${baseUrl}/api/transactionparties/${transactionPartyId}`, token)
      .then((res) => {
        this.retrieveTransactionParties();
      });
    }
  }

  componentDidMount() {
    this.retrieveTransactionParties();
  }

  render() {
    return (
      <div className="transactionParties">
        <div className="headingDiv">
          <h1 className="mainHeading">Transaction Parties</h1>
          <Link to="/transactionparties/add" className="button-wrapper"><button className="button"><i className="fas fa-plus"></i>Add Transaction Party</button></Link>
        </div>
        <div className="mainContent">
          <div className="dashboardList">
            <h2 className="listHeading">My Transaction Parties</h2>
            <div className="separator"></div>
            <table className="dataTable">
              <thead>
                <tr>
                  <th>Party Name</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {this.state.transactionParties.map((value, index) => {
                  return (
                    <tr key={value.transactionPartyId}>
                      <td>{value.transactionPartyName}</td>
                      <td><button className="delete" onClick={() => this.deleteTransactionParty(value.transactionPartyId, value.transactionPartyName)}><i className="fas fa-trash"></i></button></td>
                    </tr>
                )})}
              </tbody>
            </table>
          </div>
          <ul className="entityList">
            
          </ul>
        </div>
      </div>
    )
  }
}

export default TransactionParties;