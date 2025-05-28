import { Delivery } from "../../types/Delivery";
import { FaTruck } from "react-icons/fa";

const DeliveryInfo = ({ info }: { info: Delivery[] }) => {
  return (
    <div
      className="position-absolute bg-light border rounded p-3 shadow"
      style={{ zIndex: 1000, minWidth: "300px", top: "100%", left: 0 }}
    >
      {info.map((delivery, index) => (
        <div key={delivery.minPriceOrder} className="text-dark small mb-3">
          <div className="d-flex align-items-center mb-1">
            <FaTruck className="me-2 text-primary" />
            <strong>Order between:</strong>&nbsp;
            {delivery.minPriceOrder} - {delivery.maxPriceOrder}
          </div>
          <div className="ms-4">
            <strong>Delivery:</strong> {delivery.price}
          </div>

          {index < info.length - 1 && (
            <hr className="my-2" style={{ borderTop: "2px solid gold" }} />
          )}
        </div>
      ))}
    </div>
  );
};

export default DeliveryInfo;
