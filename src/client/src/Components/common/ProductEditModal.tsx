import React from "react";
import { Modal, Button, Form } from "react-bootstrap";

interface ProductEditModalProps {
  show: boolean;
  onHide: () => void;
  onSave: () => void;
  price: string;
  stock: number;
  onChange: (field: "price" | "stock", value: string | number) => void;
}

const ProductEditModal: React.FC<ProductEditModalProps> = ({
  show,
  onHide,
  onSave,
  price,
  stock,
  onChange,
}) => {
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave();
  };

  const handlePriceChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value.replace(",", ".");
    onChange("price", value);
  };

  return (
    <Modal show={show} onHide={onHide} backdrop="static" centered>
      <Form onSubmit={handleSubmit}>
        <Modal.Header closeButton>
          <Modal.Title>Edit Product</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form.Group className="mb-3">
            <Form.Label>Price</Form.Label>
            <Form.Control
              type="text"
              value={price}
              onChange={handlePriceChange}
              min={0}
              inputMode="decimal"
            />
          </Form.Group>
          <Form.Group>
            <Form.Label>Stock</Form.Label>
            <Form.Control
              type="number"
              value={stock}
              onChange={(e) =>
                onChange("stock", parseInt(e.target.value, 10) || 0)
              }
              min={0}
            />
          </Form.Group>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={onHide}>
            Cancel
          </Button>
          <Button variant="primary" type="submit">
            Save Changes
          </Button>
        </Modal.Footer>
      </Form>
    </Modal>
  );
};

export default ProductEditModal;
