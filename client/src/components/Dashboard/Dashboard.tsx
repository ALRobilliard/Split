import React, { Component } from 'react';
import './Dashboard.css';

class Dashboard extends Component {
  render() {
    return (
      <div className="dashboard">
        <div className="headingDiv">
          <h1 className="mainHeading">Dashboard</h1>
        </div>
        <div className="mainContent">
          <div className="dashboardList">
            <h2 className="listHeading">Coming soon.</h2>
          </div>
        </div>
      </div>
    )
  }
}

export default Dashboard;