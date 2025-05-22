interface Props {
  quantity: number;
  onIncrement: () => void;
  onDecrement: () => void;
  availableQuantity: number;
}

const CartButton = ({
  quantity,
  onIncrement,
  onDecrement,
  availableQuantity,
}: Props) => {
  if (availableQuantity === 0) {
    return (
      <button className="btn btn-sm btn-outline-secondary text-black" disabled>
        Out of Stock
      </button>
    );
  }

  if (quantity === 0) {
    return (
      <button className="btn btn-primary" onClick={onIncrement}>
        Add to Cart
      </button>
    );
  }

  return (
    <div className="d-flex align-items-center justify-content-center gap-3">
      <button
        className="btn btn-sm btn-outline-secondary"
        onClick={onDecrement}
      >
        -
      </button>
      <div className="fs-6">{quantity}</div>
      <button className="btn btn-sm btn-primary" onClick={onIncrement}>
        +
      </button>
    </div>
  );
};

export default CartButton;

// className="btn btn-sm btn-outline-secondary"
