import React from 'react';
import {BrowserRouter as Router, Link, Route, Routes} from 'react-router-dom';
import Home from "./pages/Home";
import Shop from "./pages/Shop";
import Profile from "./pages/Profile";
import './App.css';

function App() {
  return (
      <Router>
        <div>
          <nav>
            <ul>
              <li><Link to ="/">Home</Link></li>
              <li><Link to={"/profile"}>Profile</Link></li>
              <li><Link to="/shop">Shop</Link></li>
            </ul>
          </nav>

          {/* Use Routes instead of Switch */}
          <Routes>
            {/* Use element prop instead of component */}
            <Route path="/" element={<Home />} />
            <Route path="/profile" element={<Profile />} />
            <Route path="/shop" element={<Shop />} />
          </Routes>
        </div>
      </Router>
  );
}

export default App;