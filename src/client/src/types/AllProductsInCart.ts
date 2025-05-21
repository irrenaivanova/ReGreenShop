import { ProductCategoryGroup } from "./ProductCategoryGroup";

export interface AllProductsInCart {
  productsByCategories: ProductCategoryGroup[];
  totalPrice: number;
  deliveryPriceProducts: number;
  deliveryMessage: string;
}
