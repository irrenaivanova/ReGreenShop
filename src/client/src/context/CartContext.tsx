import React, {
  createContext,
  useContext,
  useEffect,
  useState,
  useCallback,
} from "react";
import { cartService } from "../services/cartService";

interface CartContextType {
  cartCount: number | null;
  loading: boolean;
  refreshCartCount: () => Promise<void>;
  clearCart: () => void;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const CartProvider = ({ children }: { children: React.ReactNode }) => {
  const [cartCount, setCartCount] = useState<number | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  const refreshCartCount = useCallback(async () => {
    setLoading(true);
    try {
      const res = await cartService.getNumberOfProductsInCart();
      setCartCount(res.data.data || 0);
    } catch {
      setCartCount(0);
    } finally {
      setLoading(false);
    }
  }, []);

  const clearCart = () => {
    setCartCount(0);
    setLoading(false); // Stop spinner immediately on clear
  };

  useEffect(() => {
    refreshCartCount();
  }, [refreshCartCount]);

  return (
    <CartContext.Provider
      value={{ cartCount, loading, refreshCartCount, clearCart }}
    >
      {children}
    </CartContext.Provider>
  );
};

export const useCart = (): CartContextType => {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart must be used within a CartProvider");
  }
  return context;
};
