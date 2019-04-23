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

  setUser = (user: UserDto) => {
    this.setState({ user });
    return user != null;
  }
  
  render() {
    return (
      <div className="App">
        <Router>
          <div className="sidebar">
            <SideNavigation user={this.state.user} />
          </div>
          <div className="main">
            <RouterView user={this.state.user} setUser={this.setUser} />
          </div>
        </Router>
      </div>
    );
  }
}

export default App;
