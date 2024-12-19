import React from 'react';

function SignUpInputs({username,birthDate,email,password,handleInputChange,handleSignUpSubmit}) {
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
                    required
                />

                <a>Birth Date:</a><input name="birthDate" value={birthDate} onChange={handleInputChange} type="date" />
                <a>Username:</a><input name="username" value={username} onChange={handleInputChange} />
                <button onClick={handleSignUpSubmit}>Sign Up</button>
            </div>
        </div>
    );}
export default SignUpInputs