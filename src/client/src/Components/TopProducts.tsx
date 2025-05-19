import { useEffect, useState } from 'react';
import { productService } from '../services/productService';
import { Product } from '../types/Product';
import Spinner from './Spinner';
import ProductCard from './ProductCard';

const TopProducts = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    productService.getTopProducts()
      .then(response => {
        setProducts(response.data.data);
      })
      .catch(() => setError('Failed to load top products'))
      .finally(() => setLoading(false));
  }, []);

  const handleLike = (id: number) => {
    setProducts(prev =>
      prev.map(product =>
        product.id === id
          ? { ...product, isLiked: !product.isLiked }
          : product
      )
    );
    // TODO: call API to toggle like/dislike
  };

  const handleAddToCart = (id: number) => {
    setProducts(prev =>
      prev.map(product =>
        product.id === id
          ? { ...product, productCartQuantity: 1 }
          : product
      )
    );
    // TODO: call API to add to cart
  };

  const handleIncrement = (id: number) => {
    setProducts(prev =>
      prev.map(product =>
        product.id === id
          ? { ...product, productCartQuantity: product.productCartQuantity + 1 }
          : product
      )
    );
    // TODO: call API to increment cart quantity
  };

  const handleDecrement = (id: number) => {
    setProducts(prev =>
      prev.map(product =>
        product.id === id && product.productCartQuantity > 0
          ? { ...product, productCartQuantity: product.productCartQuantity - 1 }
          : product
      )
    );
    // TODO: call API to decrement cart quantity
  };

  if (loading) return <Spinner />;
  if (error) return <div>{error}</div>;

  return (
    <div className="container mt-4">
      <div className="row g-2">
        {products.map(product => (
          <div key={product.id} className="col-md-2 mb-2">
            <ProductCard
              product={product}
              handleLike={handleLike}
              handleAddToCart={handleAddToCart}
              handleIncrement={handleIncrement}
              handleDecrement={handleDecrement}
            />
          </div>
        ))}
      </div>
    </div>
  );
};

export default TopProducts;
