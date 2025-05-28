import { useCallback } from "react";
import { useNavigate, useParams, Link } from "react-router-dom";
import Spinner from "../common/Spinner";
import LikeButton from "../common/LikeButton";
import CartButton from "../common/CartButton";
import { productService } from "../../services/productService";
import { useProductActions } from "../../hooks/useProductActions";
import { ProductById } from "../../types/ProductById";
import { ErrorModal } from "../common/ErrorModal";
import { SuccessModal } from "../common/SuccessModal";
import { baseUrl } from "../../Constants/baseUrl";
import { BsX } from "react-icons/bs";

const ProductDetails = () => {
  const { productId } = useParams<{ productId: string }>();
  const navigate = useNavigate();

  const fetchProduct = useCallback(async () => {
    if (!productId) throw new Error("Invalid product ID");
    const response = await productService.getProductById(Number(productId));
    return [response.data.data];
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
      className="card shadow my-4 mx-auto p-4 position-relative"
      style={{ maxWidth: "900px", borderRadius: "1rem" }}
    >
      <div
        className="position-absolute"
        style={{ top: "20px", right: "20px", zIndex: 10 }}
      >
        <div className="d-flex flex-column align-items-end gap-2">
          <button
            className="btn btn-light border rounded-circle d-flex justify-content-center align-items-center"
            style={{ width: "36px", height: "36px", padding: 0 }}
            onClick={() => navigate(-1)}
            aria-label="Close"
          >
            <BsX size={24} />
          </button>
          <div
            style={{
              position: "absolute",
              top: "30px",
              right: "800px",
              transform: "scale(2)",
              zIndex: 50,
            }}
          >
            <LikeButton
              isLiked={product.isLiked}
              onToggle={() => handleLike(product.id)}
            />
          </div>
        </div>
      </div>

      {modal?.type === "error" && (
        <ErrorModal message={modal.message} onClose={() => setModal(null)} />
      )}
      {modal?.type === "success" && (
        <SuccessModal message={modal.message} onClose={() => setModal(null)} />
      )}

      <nav aria-label="breadcrumb" className="mb-4">
        <ol className="breadcrumb bg-transparent p-0 m-0 fs-5">
          {product.categories.map((category, index) => (
            <li key={category.id} className="breadcrumb-item">
              <Link
                to={
                  index === 0
                    ? `/category/${category.id}`
                    : `/subcategory/${category.id}`
                }
                className="text-primary text-decoration-underline"
              >
                {category.name}
              </Link>
            </li>
          ))}
        </ol>
      </nav>

      <div className="d-flex flex-wrap gap-4 mt-2">
        <div style={{ flex: "0 0 280px" }}>
          <img
            src={`${baseUrl}${product.imagePath}`}
            alt={product.name}
            className="img-fluid rounded"
            style={{
              maxHeight: "300px",
              objectFit: "contain",
              width: "100%",
            }}
          />
        </div>

        <div style={{ flex: 1 }}>
          <h4 className="fw-bold">{product.name}</h4>

          {product.brand && (
            <p className="fw-bold mb-1">Brand: {product.brand}</p>
          )}
          {product.origin && (
            <p className="fw-bold mb-1">Origin: {product.origin}</p>
          )}
          {product.productCode && (
            <p className="fw-bold mb-1">Code: {product.productCode}</p>
          )}

          {product.originalUrl && (
            <p className="mb-1 fw-bold">
              Available on:{" "}
              <a
                href={product.originalUrl}
                target="_blank"
                rel="noopener noreferrer"
                className="text-decoration-underline text-primary fw-semibold"
              >
                kolichka.bg
              </a>
            </p>
          )}
          <div className="d-flex flex-wrap gap-2 mt-3">
            {product.labels?.map((label, i) => (
              <span
                key={i}
                className="badge bg-danger text-light fw-semibold fs-6 py-1 px-2 text-uppercase"
              >
                {label}
              </span>
            ))}
            {product.packaging && (
              <span className="badge bg-secondary text-light fw-semibold fs-6 py-1 px-2">
                {product.packaging}
              </span>
            )}
          </div>

          <p className="fs-4 fw-bold mt-3">
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
          {product.validTo && (
            <div className="text-danger">Valid until: {product.validTo}</div>
          )}

          <div
            className="d-flex align-items-center gap-3 mt-3 fs-5"
            style={{ transform: "scale(1.4)", transformOrigin: "top left" }}
          >
            <CartButton
              quantity={product.productCartQuantity}
              onIncrement={() => handleIncrement(product.id)}
              onDecrement={() => handleDecrement(product.id)}
              availableQuantity={product.stock}
            />
          </div>
        </div>
      </div>
      <div className="mt-5">
        <h5 className="fw-bold">Description</h5>
        <p>{product.description}</p>
      </div>
    </div>
  );
};

export default ProductDetails;
