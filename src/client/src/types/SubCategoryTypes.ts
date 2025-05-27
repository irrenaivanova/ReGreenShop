export interface SubSubCategory {
  id: number;
  name: string;
}

export interface SubCategory {
  id: number;
  name: string;
  subSubCategories: SubSubCategory[];
}

export interface CategoryWithSubCategories {
  id: number;
  name: string;
  imagePath: string;
  subCategories: SubCategory[];
}
