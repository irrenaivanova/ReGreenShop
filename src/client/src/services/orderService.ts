import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const orderService = {
  getMyOrders: () => request.get("/Order/GetMyOrders"),

  makeAnOrder: (orderData: {
    firstName: string;
    lastName: string;
    fullAddress: string;
    paymentMethodId: number;
    deliveryDateTime: string; // ISO string --> new Date().toISOString()
    discountVoucherId: string | null;
  }) => request.post("/Order/MakeAnOrder", orderData),
};
