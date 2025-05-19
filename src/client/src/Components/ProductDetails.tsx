import { useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { productService } from '../services/productService';
import { Product } from '../types/Product';

const ProductDetails = () => {
  const { id } = useParams();
  const [product, setProduct] = useState<Product | null>(null);

  useEffect(() => {
    if (id) {
      productService.getProductById(Number(id)).then(res => setProduct(res.data));
    }
  }, [id]);

  if (!product) return <div>Loading...</div>;

  return (
    <div className="container py-4">
      <h2>{product.name}</h2>
      <img src={`https://yourapi.com${product.imagePath}`} alt={product.name} className="img-fluid" />
      <p>Packaging: {product.packaging}</p>
      <p>Price: {product.price} lv</p>
      {/* Add more details here */}
    </div>
  );
};

export default ProductDetails;
