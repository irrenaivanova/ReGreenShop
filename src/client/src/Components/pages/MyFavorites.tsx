import { useCallback } from "react";
import { useProductActions } from "../../hooks/useProductActions";
import { productService } from "../../services/productService";
import { ErrorModal } from "../common/ErrorModal";
import ProductGrid from "../common/ProductGrid";
import { SuccessModal } from "../common/SuccessModal";
import Spinner from "../common/Spinner";

const MyFavorites = () => {
  const fetchTopProducts = useCallback(async () => {
    const response = await productService.getMyProducts();
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
      <h3 className="mb-4">My Products</h3>

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

export default MyFavorites;
