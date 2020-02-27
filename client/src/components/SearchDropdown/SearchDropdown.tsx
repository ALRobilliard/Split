import React, { Component } from 'react';
import './SearchDropdown.css';
import { setTimeout } from 'timers';

interface IProps {
  selectedVal?: string,
  entityList: SearchEntity[],
  setLookup: (lookupVal: SearchEntity) => void,
  labelText?: string,
}

interface IState {
  showList: boolean,
  searchTerm: string,
  validEntry: boolean
}

class SearchDropdown extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {
      showList: false,
      searchTerm: '',
      validEntry: true
    };
  }
  
  searchOnFocus = (e: React.FocusEvent<HTMLInputElement>) => this.setState({ showList: true});
  searchOnBlur = (e: React.FocusEvent<HTMLInputElement>) => {
    const callback = () => this.setState({ showList: false});
    setTimeout(callback, 200);
  }
  searchOnChange = (e: React.ChangeEvent<HTMLInputElement>) => this.setState({ searchTerm: e.target.value, validEntry: false }, () => this.props.setLookup({id: undefined, name: undefined}));
  
  render() {
    return (
      <div className="searchDropdown">
        {this.props.labelText ?
          <label htmlFor="search">{this.props.labelText}</label> :
          null
        }
        <input 
          type="text" 
          name="search" 
          id="search"
          autoComplete="off"
          value={this.props.selectedVal ? this.props.selectedVal : this.state.searchTerm}
          onChange={this.searchOnChange}
          onFocus={this.searchOnFocus}
          onBlur={this.searchOnBlur}
          style={{borderColor: this.state.validEntry || this.state.searchTerm === '' ? 'unset' : '#f44336'}}
        />
        {this.state.showList ?
          <ul className="searchList">
            {this.props.entityList
            .filter(entity => {
              if (entity.name != null) {
                return entity.name.toUpperCase().startsWith(this.state.searchTerm.toUpperCase())
              }
            })
            .map((value, index) => {
                return (
                  <li 
                    key={value.id}
                    onClick={() => {this.props.setLookup({ id: value.id, name: value.name}); this.setState({validEntry: true})}}
                  >{value.name}</li>)
              })
            }
          </ul> : 
          null
        }
      </div>
    );
  }
}

export default SearchDropdown;
