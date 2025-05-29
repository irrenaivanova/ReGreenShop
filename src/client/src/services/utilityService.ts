import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const utilityService = {
  getAllDeliveryPrices: () => request.get("/Utility/GetAllDeliveryPrices"),

  getAllCities: () => request.get("/Utility/GetAllCities"),

  getAllPaymentMethods: () => request.get("/Utility/GetAllPaymentMethods"),

  getAllLabels: () => request.get("/Utility/GetAllLabels"),

  getAllVouchers: () => request.get("/Utility/GetAllVouchers"),

  getAllGreenAlternatives: () =>
    request.get("/Utility/GetAllGreenAlternatives"),
};
