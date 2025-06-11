import React from "react";
import { Modal, Button, Form } from "react-bootstrap";

interface ProductEditModalProps {
  show: boolean;
  onHide: () => void;
  onSave: () => void;
  price: number;
  stock: number;
  onChange: (field: "price" | "stock", value: number) => void;
}

const ProductEditModal: React.FC<ProductEditModalProps> = ({
  show,
  onHide,
  onSave,
  price,
  stock,
  onChange,
}) => {
  return (
    <Modal show={show} onHide={onHide} backdrop="static" centered>
      <Modal.Header closeButton>
        <Modal.Title>Edit Product</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form>
          <Form.Group className="mb-3">
            <Form.Label>Price</Form.Label>
            <Form.Control
              type="string"
              value={price}
              onChange={(e) => onChange("price", parseFloat(e.target.value))}
              min={0}
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Stock</Form.Label>
            <Form.Control
              type="number"
              value={stock}
              onChange={(e) => onChange("stock", parseInt(e.target.value))}
              min={0}
            />
          </Form.Group>
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Cancel
        </Button>
        <Button variant="primary" onClick={onSave}>
          Save Changes
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ProductEditModal;
