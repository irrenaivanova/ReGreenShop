interface Props {
  quantity: number;
  onIncrement: () => void;
  onDecrement: () => void;
}

const CartButton = ({ quantity, onIncrement, onDecrement }: Props) => {
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
        –
      </button>
      <div className="fs-6">{quantity}</div>
      <button
        className="btn btn-sm btn-outline-secondary"
        onClick={onIncrement}
      >
        +
      </button>
    </div>
  );
};

export default CartButton;
