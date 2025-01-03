import React, {useState} from 'react';
import SignUpInputs from "../components/SignUp/SignUpInputs";
import { useNavigate } from 'react-router-dom';
import '../styles/signupstyle.css'

function SignUp({onLogin}) {
    const [formData , setFormData] = useState({email: '', password: '', birthDate: Date.now(), username: ''});
    const navigate = useNavigate();
    const handleInputChange = (e) => {
        const {name, value} = e.target;
        setFormData({...formData, [name]: value});
    }

    const handleSignUpSubmit = async () => {
        try {
            const response = await fetch('https://localhost:44332/api/User/CreateUser', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(formData),
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('token', data.token);
                localStorage.setItem("user", JSON.stringify(data.user));
                onLogin();
                navigate('/profile');
                alert('Sign up successful!');
            } else {
                alert('Invalid sign up credentials.');
            }
        } catch (error) {
            console.error('Error during sign up:', error);
        }

    }
    return (
        <div className="signup-container-all">
            <div className="signup-container">
        <h2 className="signup-title">Sign Up</h2>
        <div className="sign-up-container">
            <SignUpInputs
                email={formData.email}
                password={formData.password}
                username={formData.username}
                birthDate={formData.birthDate}
                handleInputChange={handleInputChange}
                handleSignUpSubmit={handleSignUpSubmit}
            />
            <a href="/signin">Already have an account? Sign in here.</a>
        </div>
        </div>
        </div>
    );
}
export default SignUp