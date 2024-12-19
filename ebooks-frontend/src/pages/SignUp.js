import React, {useState} from 'react';
import SignUpInputs from "../components/SignUp/SignUpInputs";
import { useNavigate } from 'react-router-dom';

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
        <div>
            <h1>Sign Up Page</h1>
            <SignUpInputs
                email={formData.email}
                password={formData.password}
                username={formData.username}
                birthDate={formData.birthDate}
                handleInputChange={handleInputChange}
                handleSignUpSubmit={handleSignUpSubmit}
            />
        </div>
    );
}
export default SignUp