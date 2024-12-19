import React from 'react';

function SignInInputs({email,password,handleInputChange,handleSignInSubmit}) {
    return (
        <div>
            <div className="input-container">
                <a>Email:</a>
                <input
                    name="email"
                    value={email}
                    onChange={handleInputChange}
                    type="email"
                    required
                />

                <a>Password:</a>
                <input

                    name="password"
                    value={password}
                    onChange={handleInputChange}
                    type="password"
                    required/>

                <button onClick={handleSignInSubmit}>Sign In</button>
            </div>
        </div>
    );
}

export default SignInInputs