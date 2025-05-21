import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const orderService = {
  getMyOrders: () => request.get("/Order/GetMyOrders"),

  getPendingOrders: () => request.get("/Order/GetPendingOrders"),

  makeAnOrder: (orderData: {
    firstName: string;
    lastName: string;
    cityId: number;
    street: string;
    number: number;
    paymentMethodId: number;
    deliveryDateTime: string; // ISO string --> new Date().toISOString()
    discountVoucherId: string | null;
  }) => request.post("/Order/MakeAnOrder", orderData),

  finishAnOrder: (finishData: {
    orderId: string;
    greenModels: { id: number; quantity: number }[];
  }) => request.post("/Order/FinishAnOrder", finishData),
};
