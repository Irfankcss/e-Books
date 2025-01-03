import React, {useState} from 'react';
import SignInInputs from "../components/SignIn/SignInInputs";
import { useNavigate } from 'react-router-dom';
import '../styles/signinstyle.css'


function SignIn({onLogin}) {
    const [formData, setFormData] = useState({ email: '', password: '' });
    const navigate = useNavigate();
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData({ ...formData, [name]: value });
    };

    const handleSignInSubmit = async () => {
        try {
            const response = await fetch('https://localhost:44332/api/Auth/login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('token', data.token);
                localStorage.setItem("user", JSON.stringify(data.user));
                onLogin();
                navigate('/profile');
                alert('Login successful!');
            } else {
                alert('Invalid login credentials.');
            }
        } catch (error) {
            console.error('Error during login:', error);
        }
    };

    return (
        <div className="signin-all-container">
            <h1 className="signin-title">Sign In</h1>
        <div className="sign-in-container">
            <SignInInputs
                email={formData.email}
                password={formData.password}
                handleInputChange={handleInputChange}
                handleSignInSubmit={handleSignInSubmit}
            />
            <a href="/signup">Don't have an account? Sign up here.</a>
        </div>
        </div>
    );
}

export default SignIn;