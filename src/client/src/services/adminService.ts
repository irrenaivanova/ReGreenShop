import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const adminService = {
  finishAnOrder: (finishData: {
    orderId: string;
    greenModels: { id: number; quantity: number }[];
  }) => request.post("/Admin/FinishAnOrder", finishData),

  getPendingOrders: () => request.get("/Admin/GetPendingOrders"),
};
