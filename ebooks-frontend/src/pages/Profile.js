import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ProfileCSS from '../pages-css/ProfileCSS.css';

function Profile() {
    const [user, setUser] = useState(null);
    const [isEditing, setIsEditing] = useState(false);
    const [editedUser, setEditedUser] = useState({});
    const [books, setBooks] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const userData = localStorage.getItem('user');
        if (!userData) {
            navigate('/signin');
        } else {
            const parsedUser = JSON.parse(userData);
            setUser(parsedUser);
            setEditedUser(parsedUser);
            fetchBooks(parsedUser.id);
        }
    }, [navigate]);

    const fetchBooks = async (userId) => {
        try {
            const response = await fetch(`https://localhost:44332/api/eBookUser/${userId}/books`);
            if (response.ok) {
                const data = await response.json();
                setBooks(data);
            } else {
                setBooks([]);
            }
        } catch (error) {
            setBooks([]);
        }
    };

    const handleEditToggle = () => {
        setIsEditing(!isEditing);
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setEditedUser((prev) => ({ ...prev, [name]: value }));
    };

    const handleSave = async () => {
        try {
            const queryParams = new URLSearchParams({
                id: editedUser.id,
                username: editedUser.username || null,
                passwordHash: editedUser.passwordHash || null,
                email: editedUser.email || null,
                birthDate: editedUser.birthDate || null,
                photo: editedUser.photo || null,
                role: editedUser.role || null,
            });

            const response = await fetch(`https://localhost:44332/api/User?${queryParams.toString()}`, {
                method: 'PUT',
            });

            if (response.ok) {
                const updatedUser = await response.json();
                setUser(updatedUser);
                localStorage.setItem('user', JSON.stringify(updatedUser));
                setIsEditing(false);
            } else {
                const errorText = await response.text();
                alert(`Failed to update user: ${errorText}`);
            }
        } catch (error) {
            alert('An error occurred while updating your profile. Please try again later.');
        }
    };


    if (!user) {
        return <p>Loading...</p>;
    }

    return (
        <div className="all-container">
        <div className="profile-container">
            <div className="profile-card">
                <img
                    src={user.photo || 'https://i.imgur.com/P0wkA6K.png'}
                    alt="Profile"
                    className="profile-photo"
                />
                {!isEditing ? (
                    <>
                        <p><strong>Username:</strong> {user.username}</p>
                        <p><strong>Email:</strong> {user.email}</p>
                        <p><strong>Birth Date:</strong> {new Date(user.birthDate).toLocaleDateString()}</p>
                        <p><strong>Account Created At:</strong> {new Date(user.createdAt).toLocaleString()}</p>
                        <button className="edit-button" onClick={handleEditToggle}>Edit Profile</button>
                    </>
                ) : (
                    <div className="edit-form">
                        <label>
                            Username:
                            <input
                                type="text"
                                name="username"
                                value={editedUser.username}
                                onChange={handleInputChange}
                            />
                        </label>
                        <label>
                            Birth Date:
                            <input
                                type="date"
                                name="birthDate"
                                value={editedUser.birthDate}
                                onChange={handleInputChange}
                            />
                        </label>
                        <label>
                            Photo URL:
                            <input
                                type="text"
                                name="photo"
                                value={editedUser.photo}
                                onChange={handleInputChange}
                            />
                        </label>
                        <button className="save-button" onClick={handleSave}>Save</button>
                        <button className="cancel-button" onClick={handleEditToggle}>Cancel</button>
                    </div>
                )}
            </div>
            <div className="myBooks-container">
                <h2>Your Books:</h2>
                <div className="books-list">
                    {books.slice(0, 4).map((book) => (
                        <div key={book.id} className="book-card">
                            <img src={book.cover || 'https://i.imgur.com/3aJR2eR.png'} alt={book.title} className="book-cover" />
                            <div className="book-info">
                                <p><strong>{book.title}</strong></p>
                                <p><strong>By {book.author}</strong></p>
                            </div>
                        </div>
                    ))}
                </div>
                {books.length > 4 && (
                    <button className="view-all-button">
                        View All
                    </button>
                )}
            </div>
        </div>
        </div>
    );
}

export default Profile;
