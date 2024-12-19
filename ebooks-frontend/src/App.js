import React, { useEffect, useState } from 'react';
import { BrowserRouter as Router, Link, Route, Routes } from 'react-router-dom';
import Home from "./pages/Home";
import Shop from "./pages/Shop";
import Profile from "./pages/Profile";
import './App.css';
import SignIn from "./pages/SignIn";
import SignUp from "./pages/SignUp";

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  // Check login status on component mount
  useEffect(() => {
    const token = localStorage.getItem('token');
    setIsLoggedIn(!!token);
  }, []);

  const handleLogout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setIsLoggedIn(false);
    window.location.reload();
  };

  const handleLogin = () => {
    setIsLoggedIn(true);
  };

  return (
      <Router>
        <div>
          <nav>
            <div className="logo-container">
              <img className="logo" src="/images/eBooksNavLogo.png" alt="logo" />
            </div>
            <button className="nav-toggle" onClick={() => document.querySelector('nav ul').classList.toggle('active')}>
              <span></span>
              <span></span>
              <span></span>
            </button>
            <ul>
              <li><Link to="/">Home</Link></li>
              <li><Link to="/shop">Store</Link></li>
              {isLoggedIn ? (
                  <>
                    <li><Link to="/profile">Profile</Link></li>
                    <button className="logout-button" onClick={handleLogout}>Log out</button>
                  </>
              ) : (
                  <>
                    <li><Link to="/signin">Sign In</Link></li>
                    <li><Link to="/signup">Sign Up</Link></li>
                  </>
              )}
            </ul>
          </nav>

          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/profile" element={<Profile />} />
            <Route path="/shop" element={<Shop />} />
            <Route path="/signin" element={<SignIn onLogin={handleLogin} />} />
            <Route path="/signup" element={<SignUp onLogin={handleLogin}/>} />
          </Routes>
        </div>
      </Router>
  );
}

export default App;
