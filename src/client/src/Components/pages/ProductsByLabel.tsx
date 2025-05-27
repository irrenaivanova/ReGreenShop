import { useCallback } from "react";
import { useParams } from "react-router-dom";
import { productService } from "../../services/productService";
import ProductGrid from "../common/ProductGrid";
import { ErrorModal } from "../common/ErrorModal";
import { SuccessModal } from "../common/SuccessModal";
import { useProductActions } from "../../hooks/useProductActions";
import Spinner from "../common/Spinner";

const ProductsByLabel = () => {
  const { labelId } = useParams<{ labelId: string }>();

  const fetchProducts = useCallback(async () => {
    if (!labelId) return [];
    const response = await productService.getProductsByLabel(Number(labelId));

    const data = response.data?.data;
    return Array.isArray(data) ? data : [];
  }, [labelId]);

  const {
    products,
    loading,
    error,
    handleLike,
    handleIncrement,
    handleDecrement,
    modal,
    setModal,
  } = useProductActions(fetchProducts);

  return (
    <div className="container mt-3">
      <h4>Products</h4>

      {loading && (
        <div className="centered-spinner">
          <Spinner />
        </div>
      )}

      {error && <ErrorModal message={error} onClose={() => setModal(null)} />}

      {modal?.type === "error" && (
        <ErrorModal message={modal.message} onClose={() => setModal(null)} />
      )}

      {modal?.type === "success" && (
        <SuccessModal message={modal.message} onClose={() => setModal(null)} />
      )}

      {!loading && !error && products.length === 0 && (
        <div className="text-center text-muted my-5">
          No products found in this category.
        </div>
      )}

      {!loading && products.length > 0 && (
        <ProductGrid
          products={products}
          onLike={handleLike}
          onIncrement={handleIncrement}
          onDecrement={handleDecrement}
        />
      )}
    </div>
  );
};

export default ProductsByLabel;
