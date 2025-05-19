import { Product } from '../types/Product';
import { baseUrl } from '../Constants/baseUrl';
import { Link } from 'react-router-dom';
import LikeButton from './LikeButton';
import CartControls from './CartControls';

interface Props {
  product: Product;
  handleLike: (id: number) => void;
  handleAddToCart: (id: number) => void;
  handleIncrement: (id: number) => void;
  handleDecrement: (id: number) => void;
}

const ProductCard = ({ product, handleLike, handleAddToCart, handleIncrement, handleDecrement }: Props) => {
  return (
    <div className="card h-100 shadow-sm border-primary position-relative">
      
      {/* Image with overlay badges */}
      <div style={{ height: '150px', overflow: 'hidden', position: 'relative' }}>
        {product.labels.map(label => (
          <span key={label} className="badge bg-info text-dark position-absolute top-0 start-0 m-1 small">
            {label}
          </span>
        ))}
        <img
          src={`${baseUrl}${product.imagePath}`}
          alt={product.name}
          className="img-fluid w-100 h-100"
          style={{ objectFit: 'contain' }}
        />
        <LikeButton isLiked={product.isLiked} onToggle={() => handleLike(product.id)} />
      </div>

      {/* Card body */}
      <div className="card-body d-flex flex-column justify-content-between p-2">
        <h6 className="card-title text-center fw-bold">
          <Link to={`/products/${product.id}`} className="text-decoration-none text-dark">
            {product.name}
          </Link>
        </h6>

        <span className="badge bg-secondary small mb-2">{product.packaging}</span>

        {/* Pricing logic */}
        {product.hasPromoDiscount ? (
          <>
            <p className="mb-0 text-muted small">
              <del>{product.price.toFixed(2)} lv</del>
            </p>
            <p className="text-danger fw-bold mb-1">{product.discountPrice?.toFixed(2)} lv</p>
          </>
        ) : (
          <p className="mb-1 text-muted small">Price: {product.price.toFixed(2)} lv</p>
        )}

        {/* Special Offer */}
        {product.hasTwoForOneDiscount && (
          <span className="badge bg-danger mb-2">2 for 1 Offer!</span>
        )}

        {/* Promotion text */}
        {product.validTo && (
          <small className="text-muted">The promotion is valid to: {product.validTo}</small>
        )}

        {/* Cart controls */}
        <div className="mt-2">
          <CartControls
            quantity={product.productCartQuantity}
            onAdd={() => handleAddToCart(product.id)}
            onIncrement={() => handleIncrement(product.id)}
            onDecrement={() => handleDecrement(product.id)}
          />
        </div>
      </div>
    </div>
  );
};

export default ProductCard;
