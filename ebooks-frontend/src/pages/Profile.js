import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

function Profile() {
    const [user, setUser] = useState(null);
    const navigate = useNavigate();

    useEffect(() => {
        const userData = localStorage.getItem('user');
        if (!userData) {
            // Redirect to Sign In if no user is found
            navigate('/signin');
        } else {
            // Parse and set user data
            setUser(JSON.parse(userData));
        }
    }, [navigate]);

    if (!user) {
        // Display a loading message while checking
        return <p>Loading...</p>;
    }

    return (
        <div>
            <h1>Profile Page</h1>
            <p>Welcome, <strong>{user.username}</strong>!</p>
            <ul>
                <img src={user.photo} alt="Profile Picture" style={{ width: '200px', height: '200px' }}/>
                <li><strong>Email:</strong> {user.email}</li>
                <li><strong>Birth Date:</strong> {new Date(user.birthDate).toLocaleDateString()}</li>
                <li><strong>Role:</strong> {user.role}</li>
                <li><strong>Account Created At:</strong> {new Date(user.createdAt).toLocaleString()}</li>
            </ul>
        </div>
    );
}

export default Profile;