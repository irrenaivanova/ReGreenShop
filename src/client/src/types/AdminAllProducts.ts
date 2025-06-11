export interface AdminAllProduct {
  id: number;
  name: string;
  price: number;
  packaging: string;
  productCode: string;
  brand: string;
  origin: string;
  imagePath: string;
  discountPercentage: number | null;
  validTo: string;
  stock: number;
  labels: string[];
  categories: string[];
}
