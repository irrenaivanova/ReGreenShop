import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import OrderSummary from "../common/OrderSummary";
import VoucherSelector from "../common/VoucherSelector";
import { userService } from "../../services/userService";
import { useCallback, useEffect, useState } from "react";

interface MakeAnOrderFormInputs {
  discountVoucherId: number | null;
  firstName: string;
  lastName: string;
  cityId: number;
  street: string;
  number: number;
  paymentMethodId: number;
  deliveryDateTime: Date;
}

const Checkout = () => {
  const location = useLocation();
  const totalPrice = location.state?.totalPrice;
  const navigate = useNavigate();

  const [user, setUser] = useState<any>(null);
  const [discountVoucherId, setdiscountVoucherId] = useState<number | null>(
    null
  );

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
      discountVoucherId: voucherId,
    };
  };

  if (!user) return <div>Loading...</div>;

  return (
    <div>
      <OrderSummary totalPrice={totalPrice} />

      <VoucherSelector
        availableGreenPoints={user.totalGreenPoints}
        onSelectVoucher={(id: number | null) => setdiscountVoucherId(id)}
      />

      <button className="btn btn-success mt-3" onClick={handleSubmitOrder}>
        Submit Order
      </button>
    </div>
  );
};
export default Checkout;
