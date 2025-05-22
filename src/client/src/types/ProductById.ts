import { Category } from "./Category";

export interface ProductById {
  id: number;
  name: string;
  description: string;
  price: number;
  productCode: string;
  stock: number;
  brand: string;
  origin: string;
  packaging: string;
  originalUrl: string;
  imagePath: string;
  isLiked: boolean;
  hasPromoDiscount: boolean;
  discountPercentage: number | null;
  validTo: string;
  additionalTextForPromotion: string;
  hasTwoForOneDiscount: boolean;
  discountPrice: number | null;
  productCartQuantity: number;
  categories: Category[];
  labels: string[];
}
