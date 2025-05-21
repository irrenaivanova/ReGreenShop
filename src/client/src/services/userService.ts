import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const userService = {
  getUserInfoForOrder: () => request.get("/User/GetUserInfoForOrder"),

  getUserInfo: () => request.get("/User/GetUserInfo"),

  getTotalGreenImpact: () => request.get("/User/GetTotalGreenImpact"),

  getAllUnReadNotifications: () =>
    request.get("/User/GetAllUnReadNotifications"),

  readNotifications: () => request.get("/User/ReadNotifications"),
};
