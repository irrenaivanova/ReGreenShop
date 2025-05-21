import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const utilityService = {
  getAllDeliveryPrices: () => request.get("/Utility/GetAllDeliveryPrices"),

  getAllLabels: () => request.get("/Utility/GetAllLabels"),

  getAllGreenAlternatives: () =>
    request.get("/Utility/GetAllGreenAlternatives"),
};
