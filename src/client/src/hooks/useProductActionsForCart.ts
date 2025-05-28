import { useCart } from "../context/CartContext";
import { cartService } from "../services/cartService";

export const useProductActionsForCart = (refetchCart: () => Promise<void>) => {
  const { refreshCartCount } = useCart();

  const handleIncrement = async (id: number) => {
    try {
      await cartService.addToCart(id);
      await refreshCartCount();
      await refetchCart();
    } catch (err: any) {
      console.error("Error adding to cart", err);
    }
  };

  const handleDecrement = async (id: number) => {
    try {
      await cartService.removeFromCart(id);
      await refreshCartCount();
      await refetchCart();
    } catch (err: any) {
      console.error("Error removing from cart", err);
    }
  };

  return {
    handleIncrement,
    handleDecrement,
  };
};
