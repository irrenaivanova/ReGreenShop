import { Link } from "react-router-dom";
import { baseUrl } from "../../Constants/baseUrl";
import { Product } from "../../types/Product";
import CartButton from "./CartButton";
import LikeButton from "./LikeButton";

interface Props {
  product: Product;
  handleLike: (id: number) => void;
  handleIncrement: (id: number) => void;
  handleDecrement: (id: number) => void;
}

const ProductCard = ({
  product,
  handleLike,
  handleIncrement,
  handleDecrement,
}: Props) => {
  return (
    <div className="card h-100 shadow-sm border-light position-relative p-2">
      <div
        style={{ height: "150px", position: "relative" }}
        className="text-center"
      >
        <div className="position-absolute top-0 start-0 m-1 d-flex flex-column gap-1 align-items-start">
          {product.labels.map((label) => (
            <span key={label} className="badge bg-danger small">
              {label}
            </span>
          ))}
        </div>

        <Link to={`/product/${product.id}`}>
          <img
            src={`${baseUrl}${product.imagePath}`}
            alt={product.name}
            className="img-fluid mt-2"
            style={{ maxHeight: "120px", objectFit: "contain" }}
          />
        </Link>
        <div
          className="position-absolute top-0 end-0 m-1"
          style={{ fontSize: "1.25rem" }}
        >
          <LikeButton
            isLiked={product.isLiked}
            onToggle={() => handleLike(product.id)}
          />
        </div>
      </div>

      <div className="card-body d-flex flex-column justify-content-between p-2">
        <h6
          className="card-title text-center fw-bold fs-6 text-truncate"
          style={{ whiteSpace: "normal" }}
        >
          <Link
            to={`/products/${product.id}`}
            state={{ backgroundLocation: location }}
            className="text-decoration-none text-dark"
            title={product.name}
          >
            {product.name}
          </Link>
        </h6>
        {product.validTo && (
          <small className="text-danger text-center">
            Valid until: {product.validTo}
          </small>
        )}

        <div className="text-center mb-2">
          <span className="badge bg-secondary text-light small">
            {product.packaging}
          </span>
        </div>
        {product.hasPromoDiscount ? (
          <div className="d-flex justify-content-center gap-2 align-items-center">
            <p className="mb-0 text-primary-emphasis small text-center">
              <del>{product.price.toFixed(2)} lv</del>
            </p>
            <p className="text-danger fw-bold mb-1 text-center fs-5">
              {product.discountPrice?.toFixed(2)} lv
            </p>
          </div>
        ) : (
          <p className="mb-1 text-primary-emphasis fw-bold mb-1 text-center fs-5">
            {product.price.toFixed(2)} lv
          </p>
        )}

        <div className="d-flex justify-content-center mt-auto">
          <CartButton
            quantity={product.productCartQuantity}
            onIncrement={() => handleIncrement(product.id)}
            onDecrement={() => handleDecrement(product.id)}
            availableQuantity={product.stock}
          />
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
