import Spinner from "../common/Spinner";
import ProductGrid from "../common/ProductGrid";
import { useProductActions } from "../../hooks/useProductActions";
import { productService } from "../../services/productService";
import { ErrorModal } from "../common/ErrorModal";
import { SuccessModal } from "../common/SuccessModal";
import { useCallback } from "react";

const TopProducts = () => {
  const fetchTopProducts = useCallback(async () => {
    const response = await productService.getTopProducts();
    return response.data.data;
  }, []);

  const {
    products,
    loading,
    error,
    handleLike,
    handleIncrement,
    handleDecrement,
    modal,
    setModal,
  } = useProductActions(fetchTopProducts);

  if (loading) {
    return (
      <div className="centered-spinner">
        <Spinner />
      </div>
    );
  }

  return (
    <div className="container mt-1">
      <h3 className="mb-4">Top Products</h3>

      {error && <ErrorModal message={error} onClose={() => {}} />}

      {modal && modal.type === "error" && (
        <ErrorModal message={modal.message} onClose={() => setModal(null)} />
      )}
      {modal && modal.type === "success" && (
        <SuccessModal message={modal.message} onClose={() => setModal(null)} />
      )}

      <ProductGrid
        products={products}
        onLike={handleLike}
        onIncrement={handleIncrement}
        onDecrement={handleDecrement}
      />
    </div>
  );
};

export default TopProducts;
