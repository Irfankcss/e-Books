import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import  '../pages-css/MyLibraryCSS.css';

function MyLibrary() {
    const [books, setBooks] = useState([]);
    const [allBooks, setAllBooks] = useState([]);
    const [recommendedBooks, setRecommendedBooks] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const userData = localStorage.getItem('user');
        if (userData) {
            const parsedUser = JSON.parse(userData);
            fetchBooks(parsedUser.id);
            fetchAllBooks();
        }
    }, []);

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

    const fetchAllBooks = async () => {
        try {
            const response = await fetch(`https://localhost:44332/api/eBook`);
            if (response.ok) {
                const data = await response.json();
                setAllBooks(data);
            } else {
                setAllBooks([]);
            }
        } catch (error) {
            setAllBooks([]);
        }
    };

    useEffect(() => {

        if (allBooks.length && books.length) {
            const ownedBookIds = new Set(books.map((book) => book.id));
            const filteredBooks = allBooks.filter((book) => !ownedBookIds.has(book.id));
            setRecommendedBooks(filteredBooks.slice(0, 5));
        }
    }, [allBooks, books]);

    const handleViewMore = () => {
        navigate('/shop');
    };

    return (
        <div className="library-container">
            <h3>Owned Books:</h3>
            <div className="books-container">
                {books.map((book) => (
                    <div className="book-card-container" key={book.id}>
                        <img className="book-cover-img" src={book.cover} alt="Book cover unavailable" />
                        <div className="book-info-container">
                            <p>{book.title}</p>
                            <p>By {book.author}</p>
                        </div>
                    </div>
                ))}
            </div>

            <h3>Recommended Books:</h3>
            <div className="books-container" id="recommended">
                {recommendedBooks.map((book) => (
                    <div className="book-card-container" key={book.id}>
                        <img className="book-cover-img" src={book.cover} alt="Book cover unavailable" />
                        <div className="book-info-container">
                            <p>{book.title}</p>
                            <p>By {book.author}</p>
                        </div>
                    </div>
                ))}
                {recommendedBooks.length > 0 && (
                    <div className="book-card-container" onClick={handleViewMore} >
                        <img className="book-cover-img-more" src="https://static.thenounproject.com/png/4939915-200.png" alt="Unavailable" />
                        <div className="book-info-container">
                            <p>Browse other books</p>
                        </div>
                    </div>
                )}
            </div>

        </div>
    );
}

export default MyLibrary;
