import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const categoriesService = {
  getRootCategories: () => request.get("/Category/GetRootCategories"),
  getSubcategories: (categoryId: number) =>
    request.get(`/Category/GetSubCategoriesByRootCategory/${categoryId}`),
};
