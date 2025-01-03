import React from 'react';

function SignInInputs({email,password,handleInputChange,handleSignInSubmit}) {
    return (
        <div>
            <div className="input-container">
                <a className="email">Email:</a>
                <input
                    className="email-input"
                    name="email"
                    value={email}
                    onChange={handleInputChange}
                    type="email"
                    required
                />

                <a className="password">Password:</a>
                <input
                    className="password-input"
                    name="password"
                    value={password}
                    onChange={handleInputChange}
                    type="password"
                    required/>

                <button className="sign-in-btn" onClick={handleSignInSubmit}>Sign In</button>
            </div>
        </div>
    );
}

export default SignInInputs