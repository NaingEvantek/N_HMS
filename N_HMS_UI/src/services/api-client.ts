import axios, { AxiosRequestConfig } from "axios";

export interface QueryRequest {
  pageIndex: number;
  pageSize: number;
  sortBy?: string;
  isDescending?: boolean;
}

export interface FetchResponse<T> {
  count: number;
  next: string | null;
  results: T[];
}

const axiosInstance = axios.create({
  baseURL: "https://localhost:7167/api",
});

axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

class APIClient<T> {
  endpoint: string;

  constructor(endpoint: string) {
    this.endpoint = endpoint;
  }

  getFilter = (config: AxiosRequestConfig) => {
    return axiosInstance
      .get<T[]>(this.endpoint, config)
      .then((res) => res.data);
  };

  getAll = (config: AxiosRequestConfig) => {
    return axiosInstance
      .get<FetchResponse<T>>(this.endpoint, config)
      .then((res) => res.data);
  };

  get = (id: number | string) => {
    return axiosInstance
      .get<T>(this.endpoint + "/" + id)
      .then((res) => res.data);
  };

  list = (body: any, config?: AxiosRequestConfig) => {
    return axiosInstance
      .post<FetchResponse<T>>(`${this.endpoint}`, body, config)
      .then((res) => res.data);
  };

  // CREATE (POST /endpoint)
  create = (data: Partial<T>) => {
    return axiosInstance.post<T>(this.endpoint, data).then((res) => res.data);
  };

  // UPDATE (PUT /endpoint/{id})
  update = (data: Partial<T> & { id: number | string }) => {
    return axiosInstance
      .put<T>(`${this.endpoint}/${data.id}`, data)
      .then((res) => res.data);
  };

  // CUSTOM POST for sub-endpoints (like checkin)
  postTo = (subEndpoint: string, data: any) => {
    return axiosInstance
      .post(`${this.endpoint}/${subEndpoint}`, data)
      .then((res) => res.data);
  };

  post = (data: any) => {
    return axiosInstance.post<T>(this.endpoint, data).then((res) => res.data);
  };
}

export { axios };
export default APIClient;
