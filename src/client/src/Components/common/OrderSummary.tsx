import { MdOutlineShoppingCart } from "react-icons/md";
import { useNavigate } from "react-router-dom";

const OrderSummary = ({ totalPrice }: { totalPrice: number }) => {
  const navigate = useNavigate();

  return (
    <div className="d-flex flex-column align-items-center my-4">
      <h4 className="text-center mb-3">Finalizing Your Order</h4>

      <div
        className="d-flex flex-column bg-white text-black border border-primary rounded px-3 py-3"
        style={{ width: 350, position: "relative" }}
      >
        <div className="d-flex align-items-center gap-2 mb-1">
          <MdOutlineShoppingCart size={35} style={{ flexShrink: 0 }} />
          <span className="fw-bold">
            Order Total: {totalPrice.toFixed(2)} lv
          </span>
        </div>

        <div className="small fw-semibold mb-2 ms-4">
          (including price for delivery)
        </div>

        <span
          className="text-decoration-underline text-black small"
          style={{
            cursor: "pointer",
            alignSelf: "flex-end",
            marginTop: "auto",
          }}
          onClick={() => navigate("/cart")}
        >
          Did you forget something?
        </span>
      </div>
    </div>
  );
};

export default OrderSummary;
