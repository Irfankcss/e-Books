import React from 'react';
import '../pages-css/HomeCSS.css';
import {useNavigate} from "react-router-dom";

function Home() {
    const navigate = useNavigate();
    const user = localStorage.getItem('user');
    function handleBrowseClick() {
        return () => navigate('/shop');
    }

    function handleRegisterButton() {
        return ()=> navigate('/signup');
    }

    function handleRegisteredButton() {
        return ()=> navigate('/mylibrary');
    }

    return (
        <div className="home-page">
            <div className="welcome-top">
                <div className="welcome-left">
                    <h1>Explore eBooks.</h1>
                    <h4>A place where you can find books </h4>
                        <h4>that satisfy everyone's taste.</h4>
                    <button className="browse-books-button" onClick={handleBrowseClick()}>Browse titles</button>
                </div>
                <div className="welcome-right">
                    <img src="https://i.etsystatic.com/16895790/r/il/a289d9/4841007885/il_794xN.4841007885_potp.jpg" alt="Welcome Image" />
                </div>
            </div>
            <div className="quote-container">
                <p className="quote-text">„Books are a uniquely portable magic.”</p>
                <p className="quote-author">- Stephen King</p>
            </div>
            {!user ?<div className="register-button-container" onClick={handleRegisterButton()}>
                <p className="register-text">Create an account to get started by purchasing books and leaving reviews.</p>
            </div>:
                <div className="register-button-container" onClick={handleRegisteredButton()}>
                    <p className="register-text">Make sure you leave your critical reviews on books you read.</p>
                </div>
            }
            <div className="welcome-bottom">
                <img src="https://i.etsystatic.com/16895790/r/il/3c81b1/4537426092/il_794xN.4537426092_bj1r.jpg" alt="Image unavailable" />
            </div>

        </div>
    );
}

export default Home;
