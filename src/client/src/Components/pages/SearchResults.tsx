import { useEffect, useState, useCallback } from "react";
import { Spinner } from "react-bootstrap";
import { useLocation } from "react-router-dom";
import { productService } from "../../services/productService";
import { ErrorModal } from "../common/ErrorModal";
import ProductGrid from "../common/ProductGrid";
import { SuccessModal } from "../common/SuccessModal";
import { useProductActions } from "../../hooks/useProductActions";

const useQuery = () => {
  const { search } = useLocation();
  return new URLSearchParams(search);
};

const SearchResults = () => {
  const query = useQuery();
  const searchTerm = query.get("query")?.trim() || "";

  // fetchProducts always returns an array to avoid errors
  const fetchProducts = useCallback(async () => {
    if (!searchTerm) return [];

    const response = await productService.getProductsBySearchString(searchTerm);
    const data = response.data?.data.products;
    return Array.isArray(data) ? data : [];
  }, [searchTerm]);

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
    <div className="container mt-1">
      <h3 className="mb-4">Search Results</h3>

      {loading && (
        <div className="centered-spinner">
          <Spinner />
        </div>
      )}

      {!loading && products.length === 0 && (
        <div className="text-center text-muted my-5">
          No products found for "{searchTerm}"
        </div>
      )}

      {error && <ErrorModal message={error} onClose={() => setModal(null)} />}

      {modal?.type === "error" && (
        <ErrorModal message={modal.message} onClose={() => setModal(null)} />
      )}

      {modal?.type === "success" && (
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

export default SearchResults;
