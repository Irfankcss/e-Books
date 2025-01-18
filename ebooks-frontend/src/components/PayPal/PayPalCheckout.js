import React from 'react';
import { PayPalButtons } from "@paypal/react-paypal-js";

function PayPalCheckout() {
    return (
        <div>
            <h1>Complete Your Purchase</h1>
            <PayPalButtons
                style={{ layout: "vertical" }}
                createOrder={(data, actions) => {
                    return actions.order.create({
                        purchase_units: [
                            {
                                amount: {
                                    value: "20.00",
                                },
                            },
                        ],
                    });
                }}
                onApprove={(data, actions) => {
                    return actions.order.capture().then((details) => {
                        alert(`Transaction completed by ${details.payer.name.given_name}`);

                    });
                }}
                onError={(err) => {
                    console.error(err);
                    alert("An error occurred during the transaction.");
                }}
            />
        </div>
    );
}

export default PayPalCheckout;
