import axios from "axios";
import { baseUrl } from "../Constants/baseUrl";

export const requestFactory = () => {
  const getToken = () => {
    const auth = localStorage.getItem("auth");
    return auth ? JSON.parse(auth).accessToken : null;
  };
  const instance = axios.create({
    baseURL: baseUrl,
    timeout: 5000,
    withCredentials: true, // for the session!
  });

  instance.interceptors.request.use((config) => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    } else {
      delete config.headers.Authorization;
    }
    return config;
  });

  return {
    get: (url: string) => instance.get(url),
    post: (url: string, data: any) => instance.post(url, data),
  };
};
