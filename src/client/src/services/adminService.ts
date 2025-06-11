import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const adminService = {
  finishAnOrder: (finishData: {
    orderId: string;
    greenModels: { id: number; quantity: number }[];
  }) => request.post("/Admin/FinishAnOrder", finishData),

  getPendingOrders: () => request.get("/Admin/GetPendingOrders"),

  searchAllProducts: (getAllData: {
    page: number;
    pageSize: number;
    searchString?: string;
    minPrice?: string;
    maxPrice?: string;
    minStock?: number;
    maxStock?: number;
  }) => request.post("Admin/SearchAll", getAllData),

  getAllProducts: (getAllData: { page: number; pageSize: number }) =>
    request.get("Admin/GetAll", { params: getAllData }),
};
