import { useCallback } from "react";
import { useParams, Link } from "react-router-dom";
import Spinner from "../common/Spinner";
import LikeButton from "../common/LikeButton";
import CartButton from "../common/CartButton";
import { productService } from "../../services/productService";
import { useProductActions } from "../../hooks/useProductActions";
import { ProductById } from "../../types/ProductById";
import { ErrorModal } from "../common/ErrorModal";
import { SuccessModal } from "../common/SuccessModal";

const ProductDetails = () => {
  const { productId } = useParams<{ productId: string }>();

  const fetchProduct = useCallback(async () => {
    if (!productId) throw new Error("Invalid product ID");
    const response = await productService.getProductById(Number(productId));
    return response.data.data;
  }, [productId]);

  const {
    products,
    loading,
    error,
    handleLike,
    handleIncrement,
    handleDecrement,
    modal,
    setModal,
  } = useProductActions(fetchProduct);

  const product = products[0] as ProductById | undefined;

  if (loading) return <Spinner />;
  if (error) return <div className="alert alert-danger">{error}</div>;
  if (!product)
    return <div className="alert alert-warning">No product found</div>;

  return (
    <div
      className="card shadow my-4 mx-auto p-4"
      style={{ maxWidth: "900px", borderRadius: "1rem" }}
    >
      {/* Modals */}
      {modal?.type === "error" && (
        <ErrorModal message={modal.message} onClose={() => setModal(null)} />
      )}
      {modal?.type === "success" && (
        <SuccessModal message={modal.message} onClose={() => setModal(null)} />
      )}

      {/* Breadcrumbs */}
      <nav aria-label="breadcrumb" className="mb-3">
        <ol className="breadcrumb">
          {product.categories.map((category) => (
            <li key={category.id} className="breadcrumb-item">
              <Link to={`/products/category/${category.id}`}>
                {category.name}
              </Link>
            </li>
          ))}
          <li className="breadcrumb-item active" aria-current="page">
            {product.name}
          </li>
        </ol>
      </nav>

      <div className="d-flex gap-4">
        <div style={{ flex: "0 0 300px" }}>
          <img
            src={product.imagePath}
            alt={product.name}
            className="img-fluid rounded"
            style={{ maxHeight: "400px", objectFit: "contain" }}
          />
        </div>

        <div style={{ flex: 1 }}>
          <h2>{product.name}</h2>
          <p className="text-muted mb-1">Brand: {product.brand}</p>
          <p className="text-muted mb-1">Origin: {product.origin}</p>
          <p className="text-muted mb-1">Packaging: {product.packaging}</p>

          <p className="fs-4 fw-bold">
            Price:{" "}
            {product.hasPromoDiscount && product.discountPrice ? (
              <>
                <del className="text-danger me-2">
                  {product.price.toFixed(2)} lv
                </del>
                <span className="text-primary">
                  {product.discountPrice.toFixed(2)} lv
                </span>
              </>
            ) : (
              <span>{product.price.toFixed(2)} lv</span>
            )}
          </p>

          <div className="d-flex align-items-center gap-3 mt-3">
            <LikeButton
              isLiked={product.isLiked}
              onToggle={() => handleLike(product.id)}
            />
            <CartButton
              quantity={product.productCartQuantity}
              onIncrement={() => handleIncrement(product.id)}
              onDecrement={() => handleDecrement(product.id)}
              availableQuantity={product.stock}
            />
          </div>
        </div>
      </div>

      <div className="mt-4">
        <h5>Description</h5>
        <p>{product.description}</p>
      </div>
    </div>
  );
};

export default ProductDetails;
