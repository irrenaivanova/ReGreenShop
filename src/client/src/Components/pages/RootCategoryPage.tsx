import { useCallback, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Button } from "react-bootstrap";
import { productService } from "../../services/productService";
import ProductGrid from "../common/ProductGrid";
import { ErrorModal } from "../common/ErrorModal";
import { SuccessModal } from "../common/SuccessModal";
import { useProductActions } from "../../hooks/useProductActions";
import { FaAngleDoubleLeft, FaAngleDoubleRight } from "react-icons/fa";
import SubCategoriesList from "../common/SubCategoryList";
import Spinner from "../common/Spinner";

const RootCategoryPage = () => {
  const { categoryId } = useParams<{ categoryId: string }>();
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  useEffect(() => {
    setPage(1);
  }, [categoryId]);

  const fetchProducts = useCallback(async () => {
    if (!categoryId) return [];
    const response = await productService.getProductsByRootCategory(
      Number(categoryId),
      page
    );
    const data = response.data?.data;
    if (data?.totalPages) {
      setTotalPages(data.totalPages);
    }

    return Array.isArray(data?.products) ? data.products : [];
  }, [categoryId, page]);

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

  const handlePrevPage = () => {
    if (page > 1) setPage((p) => p - 1);
  };

  const handleNextPage = () => {
    if (page < totalPages) setPage((p) => p + 1);
  };

  return (
    <div className="container mt-3">
      <div className="mb-4">
        <SubCategoriesList rootCategoryId={Number(categoryId)} />
      </div>
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
        <>
          <ProductGrid
            products={products}
            onLike={handleLike}
            onIncrement={handleIncrement}
            onDecrement={handleDecrement}
          />

          <div className="d-flex justify-content-center align-items-center mt-4">
            <Button
              variant="primary"
              onClick={handlePrevPage}
              disabled={page === 1}
            >
              <FaAngleDoubleLeft />
            </Button>
            <span className="mx-3">
              Page {page} of {totalPages}
            </span>
            <Button
              variant="primary"
              onClick={handleNextPage}
              disabled={page === totalPages}
            >
              <FaAngleDoubleRight />
            </Button>
          </div>
        </>
      )}
    </div>
  );
};

export default RootCategoryPage;
