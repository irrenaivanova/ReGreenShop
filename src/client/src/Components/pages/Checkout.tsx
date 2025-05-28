import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import OrderSummary from "../common/OrderSummary";
import VoucherSelector from "../common/VoucherSelector";
import { userService } from "../../services/userService";
import { useCallback, useEffect, useState } from "react";

interface MakeAnOrderFormInputs {
  voucherId: number | null;
}

const Checkout = () => {
  const location = useLocation();
  const totalPrice = location.state?.totalPrice;
  const navigate = useNavigate();

  const [user, setUser] = useState<any>(null);
  const [voucherId, setVoucherId] = useState<number | null>(null);

  const fetchUserInfo = useCallback(async () => {
    try {
      const response = await userService.getUserInfoForOrder();
      setUser(response.data.data);
    } catch (err) {
      console.error("Error fetching user info", err);
    }
  }, []);

  useEffect(() => {
    fetchUserInfo();
  }, [fetchUserInfo]);

  const handleSubmitOrder = () => {
    const orderInput: MakeAnOrderFormInputs = {
      voucherId: voucherId,
    };
  };

  if (!user) return <div>Loading...</div>;

  return (
    <div>
      <OrderSummary totalPrice={totalPrice} />

      <VoucherSelector
        availableGreenPoints={user.totalGreenPoints}
        onSelectVoucher={(id: number | null) => setVoucherId(id)}
      />

      <button className="btn btn-success mt-3" onClick={handleSubmitOrder}>
        Submit Order
      </button>
    </div>
  );
};
export default Checkout;
