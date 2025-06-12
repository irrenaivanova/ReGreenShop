import React from "react";
import { Modal, Button } from "react-bootstrap";
import Logo from "./Logo";

interface OnlinePaymentModalProps {
  show: boolean;
  onClose: () => void;
  onProceedToPayment: () => void;
}

const OnlinePaymentModal: React.FC<OnlinePaymentModalProps> = ({
  show,
  onClose,
  onProceedToPayment,
}) => {
  return (
    <Modal
      show={show}
      onHide={onClose}
      backdrop="static"
      centered
      contentClassName="bg-light"
    >
      <Modal.Header closeButton className="align-items-center">
        <div className="d-flex align-items-center gap-3">
          <div className="d-flex flex-column align-items-center">
            <Logo style={{ height: "100px", width: "auto" }} />
            <div
              className="fw-bold text-primary"
              style={{ fontSize: "0.9rem" }}
            >
              Shop Smart. Live Green.
            </div>
          </div>
          <Modal.Title className="mb-0 text-primary">
            Completing Payment ...
          </Modal.Title>
        </div>
      </Modal.Header>
      <Modal.Body>
        <p>Your order was placed successfully.</p>
        <p>Please click below to proceed to pay by card.</p>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onClose}>
          Cancel
        </Button>
        <Button variant="primary" onClick={onProceedToPayment}>
          Pay Now
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default OnlinePaymentModal;
