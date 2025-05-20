import axios from "axios";
import { baseUrl } from "../Constants/baseUrl";

export const requestFactory = () => {
  const token = localStorage.getItem("jwt"); // assuming you store it there

  const instance = axios.create({
    baseURL: baseUrl,
    timeout: 5000,
    headers: token ? { Authorization: `Bearer ${token}` } : undefined,
  });

  return {
    get: (url: string) => instance.get(url),
    post: (url: string, data: any) => instance.post(url, data),
  };
};
