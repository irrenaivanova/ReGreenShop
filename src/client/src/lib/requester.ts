import axios from 'axios';
import { baseUrl } from '../Constants/baseUrl';

export const requestFactory = () => {
  const instance = axios.create({
    baseURL: baseUrl, 
    timeout: 5000,
  });

  return {
    get: (url: string) => instance.get(url),
    post: (url: string, data: any) => instance.post(url, data),
  };
};