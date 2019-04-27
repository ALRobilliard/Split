import React, { Component } from 'react';
import './TransactionParties.css';
import { Link } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { getData } from '../../helpers/utils';

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
          <ul className="entityList">
            {this.state.transactionParties.map((value, index) => {
              return <li key={value.transactionPartyId}>{value.transactionPartyName}</li>
            })}
          </ul>
        </div>
      </div>
    )
  }
}

export default TransactionParties;