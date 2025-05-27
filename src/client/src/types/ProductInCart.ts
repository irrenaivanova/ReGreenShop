export interface ProductInCart {
  id: number;
  name: string;
  price: number;
  packaging: string;
  imagePath: string;
  isLiked?: boolean;
  stock: number;
  hasPromoDiscount: boolean;
  discountPercentage: number | null;
  validTo?: string;
  additionalTextForPromotion?: string;
  hasTwoForOneDiscount: boolean;
  discountPrice: number | null;
  productCartQuantity: number;
  quantityInCart: number;
  totalPriceProduct?: number;
  labels?: string[];
}
