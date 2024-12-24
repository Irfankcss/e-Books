import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../pages-css/MyLibraryCSS.css';

function MyLibrary() {
    const [books, setBooks] = useState([]);
    const [allBooks, setAllBooks] = useState([]);
    const [recommendedBooks, setRecommendedBooks] = useState([]);
    const [selectedBook, setSelectedBook] = useState(null);
    const [reviewForm, setReviewForm] = useState({ content: '', rating: 0 });
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
            setRecommendedBooks(filteredBooks.slice(0, 4));
        }
    }, [allBooks, books]);

    const handleViewMore = () => {
        navigate('/shop');
    };

    const handleReviewClick = (book) => {
        setSelectedBook(book);
        setReviewForm({ content: '', rating: 0 });
    };

    const handleFormChange = (e) => {
        const { name, value } = e.target;
        setReviewForm({ ...reviewForm, [name]: value });
    };

    const handleReviewSubmit = async (e) => {
        e.preventDefault();
        const userData = JSON.parse(localStorage.getItem('user'));

        if (!userData) return alert('User not logged in.');

        const reviewData = {
            content: reviewForm.content,
            rating: parseInt(reviewForm.rating, 10),
            userId: userData.id,
            eBookId: selectedBook.id,
        };

        try {
            const response = await fetch('https://localhost:44332/api/Review', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(reviewData),
            });

            if (response.ok) {
                alert('Review submitted successfully!');
                setSelectedBook(null);
            } else {
                alert('Failed to submit review.');
            }
        } catch (error) {
            alert('An error occurred while submitting the review.');
        }
    };

    function handleOpenBook(id) {
        return () => {
            navigate(`/ebook/${id}`);
        };
    }

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
                            <button
                                className="review-button"
                                onClick={() => handleReviewClick(book)}
                            >
                                Leave a Review
                            </button>
                        </div>
                    </div>
                ))}
            </div>

            <h3>Recommended Books:</h3>
            <div className="books-container" id="recommended">
                {recommendedBooks.map((book) => (
                    <div className="book-card-container" key={book.id} onClick={handleOpenBook(book.id)}>
                        <img className="book-cover-img" src={book.cover} alt="Book cover unavailable" />
                        <div className="book-info-container">
                            <p>{book.title}</p>
                            <p>By {book.author}</p>
                        </div>
                    </div>
                ))}
                {recommendedBooks.length > 0 && (
                    <div className="book-card-container" onClick={handleViewMore}>
                        <img
                            className="book-cover-img-more"
                            src="https://static.thenounproject.com/png/4939915-200.png"
                            alt="Unavailable"
                        />
                        <div className="book-info-container">
                            <p>Browse other books</p>
                        </div>
                    </div>
                )}
            </div>

            {selectedBook && (
                <div className="review-form-modal">
                    <form className="review-form" onSubmit={handleReviewSubmit}>
                        <h3>Review for {selectedBook.title}</h3>
                        <textarea
                            name="content"
                            value={reviewForm.content}
                            onChange={handleFormChange}
                            placeholder="Write your review..."
                            required
                        ></textarea>
                        <input
                            type="number"
                            name="rating"
                            className="rating-input"
                            value={reviewForm.rating}
                            onChange={handleFormChange}
                            min="1"
                            max="5"
                            placeholder="Rating (1-5)"
                            required
                        />
                        <button type="submit" className="submit-button">Submit Review</button>
                        <button type="button" className="cancel-button" onClick={() => setSelectedBook(null)}>
                            Cancel
                        </button>
                    </form>
                </div>
            )}
        </div>
    );
}

export default MyLibrary;
