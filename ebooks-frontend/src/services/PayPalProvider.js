import React from 'react';
import ReactDOM from 'react-dom';
import { PayPalScriptProvider } from "@paypal/react-paypal-js";
import App from '../App';

const initialOptions = {
    "client-id": "ATeGTLOFyvFOctWSiQ78_HLtmJ7ksJf3mSDyaXZyqbGZwrPwkj8LBpv8ndtZ4g6HBr8hmYR9rlvtFBcB",
    currency: "USD",
};

ReactDOM.render(
    <PayPalScriptProvider options={initialOptions}>
        <App />
    </PayPalScriptProvider>,
    document.getElementById('root')
);
