interface Props {
    quantity: number;
    onAdd: () => void;
    onIncrement: () => void;
    onDecrement: () => void;
  }
  
  const CartControls = ({ quantity, onAdd, onIncrement, onDecrement }: Props) => {
    if (quantity === 0) {
      return (
        <button className="btn btn-sm btn-success w-100" onClick={onAdd}>
          Add to Cart
        </button>
      );
    }
  
    return (
      <div className="d-flex align-items-center justify-content-between">
        <button className="btn btn-sm btn-outline-secondary" onClick={onDecrement}>-</button>
        <span>{quantity}</span>
        <button className="btn btn-sm btn-outline-secondary" onClick={onIncrement}>+</button>
      </div>
    );
  };
  
  export default CartControls;