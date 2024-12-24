import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import '../pages-css/EbookCSS.css';

function Ebook() {
    const { id } = useParams();
    const [book, setBook] = useState(null);
    const [reviews, setReviews] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchBookDetails = async () => {
            try {
                const bookResponse = await fetch(`https://localhost:44332/api/eBook/${id}`);
                const reviewsResponse = await fetch(`https://localhost:44332/api/Review/byBookId?bookId=${id}`);

                if (bookResponse.ok) {
                    const bookData = await bookResponse.json();
                    setBook(bookData);
                }
                if (reviewsResponse.ok) {
                    const reviewsData = await reviewsResponse.json();
                    setReviews(reviewsData);
                }
            } catch (err) {
                console.error('Error fetching data:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchBookDetails();
    }, [id]);

    if (loading) return <p>Loading...</p>;

    if (!book) return <p>Book not found</p>;

    return (
        <div className="ebook-details">
            <div className="ebook-info">
                <img src={book.cover} alt={book.title} className="ebook-cover" />
                <div className="ebook-metadata">
                    <h1 className="ebook-title">{book.title}</h1>
                    <p className="ebook-author">By: {book.author}</p>
                    <p className="ebook-description">{book.description}</p>
                    <p className="ebook-published">Published: {new Date(book.publishedDate).toLocaleDateString()}</p>
                    <p className="ebook-price">Price: ${book.price.toFixed(2)}</p>
                    <button className="purchase-button">Purchase</button>
                </div>
            </div>
            <div className="reviews-section">
                <h3>Reviews</h3>
                {reviews.length > 0 ? (
                    <ul className="reviews-list">
                        {reviews.map((review, index) => (
                            <li key={index} className="review-item">
                                <p className="review-username">{review.user.username}</p>
                                <p className="review-rating">Rating: {review.rating}</p>
                                <p className="review-content">{review.content}</p>
                            </li>
                        ))}
                    </ul>
                ) : (
                    <p className="no-reviews-message">No reviews yet. Purchase to leave a review.</p>
                )}
            </div>
        </div>

    );
}

export default Ebook;
