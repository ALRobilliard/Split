import React, { Component } from 'react';
import './AddTransactionParty.css';
import { Link, BrowserRouter as Router, Redirect } from 'react-router-dom';

import { baseUrl } from '../../private/config';
import { postData } from '../../helpers/utils';

interface IProps {
  user?: UserDto
}

interface IState {
  partyName: string,
  redirect: boolean
}

class AddTransactionParty extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      partyName: '',
      redirect: false
    };
  }

  partyNameOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ partyName: e.target.value });

  postParty = () => {
    const partyName = this.state.partyName,
      token = 
        this.props.user != null ?
        this.props.user.token :
        undefined,
      transactionPartyObj = {
        TransactionPartyName: partyName
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
            <label htmlFor="defaultCat">Default Category:</label>
            <select name="defaultCat" id="defaultCat">
              <option value="-1">TODO</option>
            </select>
          </div>
          <div className="form-actions">
            <Link to="/transactionparties"><button className="button button-close">Cancel</button></Link>
            <a><button className="button button-confirm" onClick={this.postParty}>Create</button></a>
          </div>
        </div>
      </div>
    )
  }
}

export default AddTransactionParty;