export interface Product {
    id: number;
    name: string;
    price: number;
    packaging: string;
    imagePath: string;
    isLiked: boolean;
    hasPromoDiscount: boolean;
    discountPercentage: number | null;
    validTo: string;
    additionalTextForPromotion: string;
    hasTwoForOneDiscount: boolean;
    discountPrice: number | null;
    productCartQuantity: number;
    labels: string[];
  }