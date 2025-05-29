import { useLocation, useNavigate } from "react-router-dom";
import OrderSummary from "../common/OrderSummary";
import VoucherSelector from "../common/VoucherSelector";
import CheckoutForm from "../common/CheckoutForm";
import { userService } from "../../services/userService";
import { orderService } from "../../services/orderService";
import { useCallback, useEffect, useState } from "react";
import { useModal } from "../../context/ModalContext";
import { UserInfoForOrder } from "../../types/UserInfoForOrder";
import { useCart } from "../../context/CartContext";

const Checkout = () => {
  const location = useLocation();
  const totalPrice = location.state?.totalPrice;
  const navigate = useNavigate();
  const { showModal } = useModal();

  const [user, setUser] = useState<UserInfoForOrder | null>(null);
  const [discountVoucherId, setdiscountVoucherId] = useState<number | null>(
    null
  );
  const { clearCart } = useCart();
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

  const handleFormSubmit = async (data: any) => {
    try {
      const orderData = {
        ...data,
        discountVoucherId,
        deliveryDateTime: data.deliveryDateTime.toISOString(),
      };

      const response = await orderService.makeAnOrder(orderData);
      showModal?.("success", response.data.message || "Order placed!");
      clearCart();
      navigate("/");
    } catch (error: any) {
      const msg = error.response?.data?.error || "Order failed.";
      showModal?.("error", msg);
    }
  };

  if (!user) return <div>Loading...</div>;

  return (
    <div>
      <OrderSummary totalPrice={totalPrice} />

      <VoucherSelector
        availableGreenPoints={user.totalGreenPoints}
        onSelectVoucher={(id: number | null) => setdiscountVoucherId(id)}
      />

      <CheckoutForm userInfo={user} onFormSubmit={handleFormSubmit} />
    </div>
  );
};

export default Checkout;
