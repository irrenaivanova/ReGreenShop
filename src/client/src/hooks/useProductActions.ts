import { useState, useCallback, useEffect } from "react";
import { useAuth } from "../context/AuthContext";
import { useCart } from "../context/CartContext";
import { cartService } from "../services/cartService";
import { productService } from "../services/productService";
import { Product } from "../types/Product";

export const useProductActions = (fetchProducts: () => Promise<Product[]>) => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [modal, setModal] = useState<{
    type: "success" | "error";
    message: string;
  } | null>(null);
  const { isAuthenticated } = useAuth();
  const { refreshCartCount } = useCart();
  const refetch = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const fresh = await fetchProducts();
      const freshProducts = Array.isArray(fresh) ? fresh : [fresh];
      setProducts(freshProducts);
    } catch (error) {
      setProducts([]);
      setError("Failed to load products");
      setModal({ type: "error", message: "Failed to load products" });
      console.error("Failed to fetch products:", error);
    } finally {
      setLoading(false);
    }
  }, [fetchProducts]);

  useEffect(() => {
    refetch();
  }, [isAuthenticated, refetch]);

  const handleLike = async (id: number) => {
    if (!isAuthenticated) {
      setModal({ type: "error", message: "Please log in to like products" });
      return;
    }

    setProducts((prev) =>
      prev.map((product) =>
        product.id === id ? { ...product, isLiked: !product.isLiked } : product
      )
    );

    try {
      await productService.likeAProduct(id);
      setModal({ type: "success", message: "Favorites updated" });
    } catch (err: any) {
      setProducts((prev) =>
        prev.map((product) =>
          product.id === id
            ? { ...product, isLiked: !product.isLiked }
            : product
        )
      );

      const errorMessage =
        err?.response?.status === 401
          ? "Please log in to like products"
          : err?.response?.data?.error || "Something went wrong";

      setModal({ type: "error", message: errorMessage });
    }
  };

  const handleIncrement = async (id: number) => {
    setProducts((prev) =>
      prev.map((product) =>
        product.id === id
          ? {
              ...product,
              productCartQuantity: (product.productCartQuantity || 0) + 1,
            }
          : product
      )
    );

    try {
      await cartService.addToCart(id);
      setModal({ type: "success", message: "Product added to cart" });
      await refreshCartCount();
    } catch (err: any) {
      setProducts((prev) =>
        prev.map((product) =>
          product.id === id &&
          product.productCartQuantity &&
          product.productCartQuantity > 0
            ? {
                ...product,
                productCartQuantity: product.productCartQuantity - 1,
              }
            : product
        )
      );

      const errorMessage =
        err?.response?.data?.error || "Failed to add product to cart";

      setModal({ type: "error", message: errorMessage });
    }
  };

  const handleDecrement = async (id: number) => {
    const product = products.find((p) => p.id === id);
    if (
      !product ||
      !product.productCartQuantity ||
      product.productCartQuantity === 0
    )
      return;

    setProducts((prev) =>
      prev.map((product) =>
        product.id === id
          ? { ...product, productCartQuantity: product.productCartQuantity - 1 }
          : product
      )
    );

    try {
      await cartService.removeFromCart(id);
      setModal({ type: "success", message: "Product removed from cart" });
      await refreshCartCount();
    } catch (err: any) {
      setProducts((prev) =>
        prev.map((product) =>
          product.id === id
            ? {
                ...product,
                productCartQuantity: (product.productCartQuantity || 0) + 1,
              }
            : product
        )
      );

      const errorMessage =
        err?.response?.data?.error || "Failed to remove product from cart";

      setModal({ type: "error", message: errorMessage });
    }
  };

  return {
    products,
    setProducts,
    loading,
    error,
    handleLike,
    handleIncrement,
    handleDecrement,
    modal,
    setModal,
    refetch,
  };
};
