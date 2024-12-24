import React from 'react';
import {useNavigate} from "react-router-dom";

function Logo(){
    const navigate = useNavigate();
    function navigateHome() {
            return ()=> navigate('/');
    }

    return (
        <div className="logo-container">
            <img className="logo" src="/images/eBooksNavLogo.png" alt="logo" onClick={navigateHome()}/>
        </div>
    );
}
export default Logo;