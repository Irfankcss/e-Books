import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import '../pages-css/EbookCSS.css';
import { PayPalScriptProvider, PayPalButtons } from '@paypal/react-paypal-js';

function Ebook() {
    const { id } = useParams();
    const [book, setBook] = useState(null);
    const [reviews, setReviews] = useState([]);
    const [loading, setLoading] = useState(true);
    const [purchaseComplete, setPurchaseComplete] = useState(false);

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

    const handleApprove = (orderID) => {
        console.log('Order approved with ID:', orderID);
        setPurchaseComplete(true);
    };

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

                    {purchaseComplete ? (
                        <p className="purchase-success">Thank you for your purchase!</p>
                    ) : (
                        <PayPalScriptProvider options={{ "client-id": "ATeGTLOFyvFOctWSiQ78_HLtmJ7ksJf3mSDyaXZyqbGZwrPwkj8LBpv8ndtZ4g6HBr8hmYR9rlvtFBcB" }}>
                            <PayPalButtons
                                style={{ layout: 'vertical' }}
                                createOrder={(data, actions) => {
                                    return actions.order.create({
                                        purchase_units: [
                                            {
                                                amount: {
                                                    value: book.price.toFixed(2),
                                                },
                                            },
                                        ],
                                    });
                                }}
                                onApprove={(data, actions) => {
                                    return actions.order.capture().then((details) => {
                                        handleApprove(data.orderID);
                                    });
                                }}
                                onError={(err) => {
                                    console.error('PayPal Checkout Error:', err);
                                }}
                            />
                        </PayPalScriptProvider>
                    )}
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
