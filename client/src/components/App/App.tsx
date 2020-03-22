import React, { Component } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import './App.css';
import SideNavigation from '../SideNavigation/SideNavigation';
import RouterView from '../RouterView/RouterView';

interface IProps {
  // No props as this is the top-level component.
}

interface IState {
  user?: UserDto
}

class App extends Component<IProps, IState> {
  state: IState;

  constructor(props: IProps) {
    super(props);
    this.state = {};
  }

  refreshUser = () => {
    const user: UserDto = {
      id: sessionStorage.getItem('userId') as string,
      firstName: sessionStorage.getItem('userFirstName') as string,
      lastName: sessionStorage.getItem('userLastName') as string,
      email: sessionStorage.getItem('userEmail') as string,
      token: sessionStorage.getItem('userToken') as string
    };
    
    if (user.token == null) {
      this.setState({ user: undefined});
      return false;
    }

    this.setState({ user });
    return true;
  }

  clearUser = () => {
    sessionStorage.removeItem('userId');
    sessionStorage.removeItem('userFirstName');
    sessionStorage.removeItem('userLastName');
    sessionStorage.removeItem('userEmail');
    sessionStorage.removeItem('userToken');

    this.refreshUser();
  }

  componentWillMount() {
    this.refreshUser();
  }
  
  render() {
    return (
      <div className="App">
        <Router>
          <div className="sidebar">
            <SideNavigation user={this.state.user} clearUser={this.clearUser} />
          </div>
          <div className="main">
            <RouterView user={this.state.user} refreshUser={this.refreshUser} />
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
