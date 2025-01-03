import React from 'react';

function SignUpInputs({username,birthDate,email,password,handleInputChange,handleSignUpSubmit}) {
    return (
        <div className="signup-input-container">
            <a className="signup-label">Email:</a>
            <input
                className="signup-input"
                name="email"
                value={email}
                onChange={handleInputChange}
                type="email"
                required
            />
            <a className="signup-label">Password:</a>
            <input
                className="signup-input"
                name="password"
                value={password}
                onChange={handleInputChange}
                type="password"
                required
            />
            <a className="signup-label">Confirm password:</a>
            <input className="signup-input" type="password" required />
            <a className="signup-label">Birth Date:</a>
            <input
                name="birthDate"
                className="signup-input"
                value={birthDate}
                onChange={handleInputChange}
                type="date"
            />
            <a className="signup-label">Username:</a>
            <input
                className="signup-input"
                name="username"
                value={username}
                onChange={handleInputChange}
            />
            <button className="signup-btn" onClick={handleSignUpSubmit}>
                Sign Up
            </button>
        </div>
    );}
export default SignUpInputs