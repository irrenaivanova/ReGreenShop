import { Product } from "../types/Product";
import ProductCard from "./ProductCard";

type Props = {
  products: Product[];
  onLike: (id: number) => void;
  onIncrement: (id: number) => void;
  onDecrement: (id: number) => void;
};

const ProductGrid = ({ products, onLike, onIncrement, onDecrement }: Props) => {
  return (
    <div className="row g-3">
      {products.map((product) => (
        <div key={product.id} className="col-md-2 mb-2">
          <ProductCard
            product={product}
            handleLike={onLike}
            handleIncrement={onIncrement}
            handleDecrement={onDecrement}
          />
        </div>
      ))}
    </div>
  );
};

export default ProductGrid;
