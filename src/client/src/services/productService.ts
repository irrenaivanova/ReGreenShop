import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const productService = {
  getProductById: (id: number) => request.get(`/Product/${id}`),

  getTopProducts: () => request.get("/Product/GetTopProducts"),

  getProductsByLabel: (id: number) =>
    request.get(`/Product/ProductsByLabel/${id}`),

  getProductsBySubCategory: (categoryId: number) =>
    request.get("/Product/ProductsBySubCategory", {
      params: { id: categoryId },
    }),

  getProductsByRootCategory: (categoryId: number, page: number = 1) =>
    request.get("/Product/ProductsByRootCategory", {
      params: { CategoryId: categoryId, Page: page },
    }),

  getProductsBySearchString: (searchString: string) =>
    request.get(`/Product/ProductsBySearchString?SearchString=${searchString}`),

  getMyProducts: () => request.get("/Product/GetMyProducts"),

  likeAProduct: (id: number) => request.get(`/Product/LikeAProduct/${id}`),
};
