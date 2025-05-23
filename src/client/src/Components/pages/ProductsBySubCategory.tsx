import { useCallback } from "react";
import { useParams } from "react-router-dom";
import { Spinner } from "react-bootstrap";
import { productService } from "../../services/productService";
import ProductGrid from "../common/ProductGrid";
import { ErrorModal } from "../common/ErrorModal";
import { SuccessModal } from "../common/SuccessModal";
import { useProductActions } from "../../hooks/useProductActions";

const ProductsBySubCategory = () => {
  const { categoryId } = useParams<{ categoryId: string }>();

  const fetchProducts = useCallback(async () => {
    if (!categoryId) return [];
    const response = await productService.getProductsByRootCategory(
      Number(categoryId)
    );

    const data = response.data?.data.products;
    return Array.isArray(data) ? data : [];
  }, [categoryId]);

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

export default ProductsBySubCategory;
