import { useEffect, useState } from "react";
import { productService } from "../services/productService";
import { Product } from "../types/Product";
import Spinner from "./Spinner";
import ProductCard from "./ProductCard";
import { SuccessModal } from "./SuccessModal";
import { ErrorModal } from "./ErrorModal";

const TopProducts = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [modal, setModal] = useState<{
    type: "success" | "error";
    message: string;
  } | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    productService
      .getTopProducts()
      .then((response) => {
        setProducts(response.data.data);
      })
      .catch(() => setError("Failed to load top products"))
      .finally(() => setLoading(false));
  }, []);

  const handleLike = async (id: number) => {
    // Optimistically update the UI
    setProducts((prev) =>
      prev.map((product) =>
        product.id === id ? { ...product, isLiked: !product.isLiked } : product
      )
    );

    try {
      const response = await productService.likeAProduct(id);

      // You may adjust based on your backend's actual shape
      const result = response.data;

      setModal({
        type: "success",
        message: result.message || "Like status updated!",
      });

      // Optional: re-fetch products to keep in sync
      // const updated = await productService.getTopProducts();
      // setProducts(updated.data.data);
    } catch (err: any) {
      setModal({
        type: "error",
        message: err.response?.data?.message || "Error toggling like",
      });

      // Revert optimistic update if needed
      setProducts((prev) =>
        prev.map((product) =>
          product.id === id
            ? { ...product, isLiked: !product.isLiked }
            : product
        )
      );
    }
  };

  const handleAddToCart = (id: number) => {
    setProducts((prev) =>
      prev.map((product) =>
        product.id === id ? { ...product, productCartQuantity: 1 } : product
      )
    );
    // TODO: call API to add to cart
  };

  const handleIncrement = (id: number) => {
    setProducts((prev) =>
      prev.map((product) =>
        product.id === id
          ? { ...product, productCartQuantity: product.productCartQuantity + 1 }
          : product
      )
    );
    // TODO: call API to increment cart quantity
  };

  const handleDecrement = (id: number) => {
    setProducts((prev) =>
      prev.map((product) =>
        product.id === id && product.productCartQuantity > 0
          ? { ...product, productCartQuantity: product.productCartQuantity - 1 }
          : product
      )
    );
    // TODO: call API to decrement cart quantity
  };

  if (loading) return <Spinner />;
  if (error) return <div>{error}</div>;

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Top Products</h2>
      <div className="row g-3">
        {products.map((product) => (
          <div key={product.id} className="col-md-2 mb-2">
            <ProductCard
              product={product}
              handleLike={handleLike}
              handleAddToCart={handleAddToCart}
              handleIncrement={handleIncrement}
              handleDecrement={handleDecrement}
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default TopProducts;
