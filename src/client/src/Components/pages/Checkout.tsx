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
import OnlinePaymentModal from "../common/OnlinePaymentModal";
import { cartService } from "../../services/cartService";
import { loadStripe } from "@stripe/stripe-js";

const Checkout = () => {
  const stripePromise = loadStripe(
    "pk_test_51RZ67j4YVlRoXl5LiZP5WREzzhKAdPf2VCjW6wdNxChxvblFNgURDbkIVEOvtedC8c8Zrowi2eVm4saxE7T3DtWD00nYRzQ6pl"
  );
  const ONLINE_PAYMENT_ID = 1;
  const location = useLocation();
  const totalPrice = location.state?.totalPrice;
  const navigate = useNavigate();
  const { showModal } = useModal();

  const [user, setUser] = useState<UserInfoForOrder | null>(null);
  const [discountVoucherId, setdiscountVoucherId] = useState<number | null>(
    null
  );
  const [showPaymentModal, setShowPaymentModal] = useState(false);
  const [createdOrderId, setCreatedOrderId] = useState<string | null>(null);
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

  const handleFormSubmit = async (data: any, paymentMethodId: number) => {
    try {
      const orderData = {
        ...data,
        discountVoucherId,
        deliveryDateTime: data.deliveryDateTime.toISOString(),
      };

      const response = await orderService.makeAnOrder(orderData);
      const orderId = response.data.data;

      if (paymentMethodId == ONLINE_PAYMENT_ID) {
        setCreatedOrderId(orderId);
        setShowPaymentModal(true);
      } else {
        showModal?.("success", response.data.message || "Order placed!");
        clearCart();
        navigate("/");
      }
    } catch (error: any) {
      const msg = error.response?.data?.error || "Order failed.";
      showModal?.("error", msg);
      navigate("/cart");
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

      <OnlinePaymentModal
        show={showPaymentModal}
        onClose={() => setShowPaymentModal(false)}
        onProceedToPayment={async () => {
          try {
            if (!createdOrderId) {
              showModal?.("error", "Order ID is missing.");
              return;
            }
            const stripe = await stripePromise;
            console.log(createdOrderId);
            const response = await cartService.createStripeSession(
              createdOrderId
            );
            const sessionId = response.data.data;
            console.log(sessionId);
            await stripe?.redirectToCheckout({ sessionId });
          } catch (err) {
            showModal?.(
              "error",
              "Failed to start Stripe payment. Your order is made but waits payment"
            );
          }
        }}
      />
    </div>
  );
};

export default Checkout;
