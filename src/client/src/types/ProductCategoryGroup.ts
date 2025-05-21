import { ProductInCart } from "./ProductInCart";

export interface ProductCategoryGroup {
  id: number;
  categoryName: string;
  products: ProductInCart[];
}
