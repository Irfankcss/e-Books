import React, {useState} from 'react';
import SignInInputs from "../components/SignIn/SignInInputs";
import { useNavigate } from 'react-router-dom';


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
        <div>
            <h1>Sign In Page</h1>
            <SignInInputs
                email={formData.email}
                password={formData.password}
                handleInputChange={handleInputChange}
                handleSignInSubmit={handleSignInSubmit}
            />
        </div>
    );
}

export default SignIn;