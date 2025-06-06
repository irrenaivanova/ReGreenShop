import { requestFactory } from "../lib/requester";

const request = requestFactory();

export const authService = {
  login: (email: string, password: string) =>
    request.post("/User/Login", { userName: email, password }),

  register: (userName: string, password: string) =>
    request.post("/User/Register", { userName, password }),
};
