import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const cartService = {
  getNumberOfProductsInCart: () => request.get("/Cart/NumberOfProductsInCart"),

  getNumberOfConcreteProductInCart: (id: number) =>
    request.get(`/Cart/NumberOfConcreteProductInCart/${id}`),

  viewProductsInCart: () => request.get("/Cart/ViewProductsInCart"),

  addToCart: (id: number) => request.get(`/Cart/AddToCart/${id}`),

  removeFromCart: (id: number) => request.get(`/Cart/RemoveFromCart/${id}`),

  cleanCart: () => request.get("/Cart/CleanCart"),

  createStripeSession: (orderId: string) =>
    request.post("Cart/CreateStripeSession", { orderId }),
};
