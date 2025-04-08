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
    const [error, setError] = useState(null);

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
                setError('Failed to load book details');
            } finally {
                setLoading(false);
            }
        };

        fetchBookDetails();
    }, [id]);

    const handleApprove = async (details) => {
        try {
            const response = await fetch("https://localhost:44332/api/Payment/process", {
                method: "POST",
                headers: { 
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${localStorage.getItem('token')}`
                },
                body: JSON.stringify({
                    bookId: book.id,
                    userId: JSON.parse(localStorage.getItem('user')).id,
                    orderId: details.id,
                    paymentId: details.id,
                    status: details.status,
                    amount: book.price
                })
            });

            if (response.ok) {
                const result = await response.json();
                setPurchaseComplete(true);
                alert("Purchase successful! Thank you for your purchase.");
            } else {
                const error = await response.json();
                throw new Error(error.message || 'Payment processing failed');
            }
        } catch (error) {
            console.error("Error during payment processing:", error);
            setError(error.message || "Error during purchase. Please try again.");
            alert("Error during purchase. Please try again.");
        }
    };

    if (loading) return <div className="loading">Loading...</div>;
    if (error) return <div className="error">{error}</div>;
    if (!book) return <div className="error">Book not found</div>;

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
                        <div className="purchase-success">
                            <h2>Thank you for your purchase!</h2>
                            <p>You can now download and enjoy your book.</p>
                        </div>
                    ) : (
                        <PayPalScriptProvider options={{ 
                            "client-id": process.env.REACT_APP_PAYPAL_CLIENT_ID,
                            currency: "USD"
                        }}>
                            <PayPalButtons
                                style={{ layout: 'vertical' }}
                                createOrder={(data, actions) => {
                                    return actions.order.create({
                                        intent: "CAPTURE",
                                        purchase_units: [
                                            {
                                                amount: {
                                                    currency_code: "USD",
                                                    value: book.price.toFixed(2)
                                                },
                                                description: `Purchase of ${book.title}`
                                            }
                                        ],
                                    });
                                }}
                                onApprove={(data, actions) => {
                                    return actions.order.capture().then((details) => {
                                        handleApprove(details);
                                    });
                                }}
                                onError={(err) => {
                                    console.error('PayPal Checkout Error:', err);
                                    setError('Payment failed. Please try again.');
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
