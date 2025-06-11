import React from "react";
import { Modal, Button } from "react-bootstrap";

interface ProductDeleteModalProps {
  show: boolean;
  onHide: () => void;
  onDelete: () => void;
  productName?: string;
}

const ProductDeleteModal: React.FC<ProductDeleteModalProps> = ({
  show,
  onHide,
  onDelete,
  productName,
}) => {
  return (
    <Modal show={show} onHide={onHide} backdrop="static" centered>
      <Modal.Header closeButton>
        <Modal.Title>Delete Product</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        Are you sure you want to delete{" "}
        <strong>{productName || "this product"}</strong>?
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Cancel
        </Button>
        <Button variant="danger" onClick={onDelete}>
          Delete
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ProductDeleteModal;
