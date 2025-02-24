export const QUERY_STALE_TIMES = {
    SHORT: 1 * 60 * 1000,  // 1 minute
    MEDIUM: 5 * 60 * 1000, // 5 minutes
    LONG: 15 * 60 * 1000,  // 15 minutes
  };

  export interface CursorPagedResponse<T>{
    data: T[];
    nextCursor: string | null;
  }
  const API_BASE_URL = "api";

  type RequestOptions = {
    method?: RequestMethod;
    body?: Record<string, any> | FormData;
    params?: Record<string, string | number>;
    auth?: boolean;
  }
  type RequestMethod = "GET" | "POST" | "PUT";

  const apiRequest = async <T>(
    endpoint: string,
    { method = "GET", body, params, auth=true}: RequestOptions = {}
  ) : Promise<T> => {
    let url = `${API_BASE_URL}/${endpoint}`;
    
    if (params){
      const searchParams = new URLSearchParams();
      Object.entries(params).forEach(([key, value]) =>
        searchParams.append(key, String(value))
      );
      url += `?${searchParams.toString()}`;
    }
    const headers:HeadersInit = { };
    if (auth) {
      const token = localStorage.getItem("authToken");
      if (token) headers.Authorization = `Bearer ${token}`;
    }
    const requestOptions: RequestInit = {
      method,
      headers,
    };
  
    if (body) {
      if (body instanceof FormData) {
        requestOptions.body = body; // Let the browser set Content-Type
      } else {
        headers["Content-Type"] = "application/json"; // Set JSON header
        requestOptions.body = JSON.stringify(body);   // Convert body to JSON
      }
    }

    const response = await fetch (url.toString(),requestOptions);

    if (!response.ok){
      throw new Error(`API Error: ${response.status} - ${response.statusText}`)
    }
    return response.json();

  };

  export const makeGetRequest = async <T>(
    endpoint: string,
    params?: Record<string, string | number>,
    auth: boolean = true
  ): Promise<T> => {
    return apiRequest(endpoint, { method: "GET", params, auth });
  };
  
  export const makeCreateRequest = async <T>(
    endpoint: string,
    body: Record<string, any>,
    auth: boolean = true
  ): Promise<T> => {
    return apiRequest(endpoint, { method: "POST", body, auth });
  };
  
  export const makeUpdateRequest = async <T>(
    endpoint: string,
    body: Record<string, any>,
    auth: boolean = true
  ): Promise<T> => {
    return apiRequest(endpoint, { method: "PUT", body, auth });
  };

  export const makePagedRequest = async <T>(
    enpdpoint: string, 
    params?: Record<string, string | number>,
    cursor?: string | null, 
    pageSize: number = 20,
    auth: boolean = true
  ): Promise<{data: T[], nextCursor: string | null}> => {
    const finalParams: Record<string, string | number> = params ?? {};
    if (cursor != null) {
      finalParams.cursor = cursor;
    }
    finalParams.pageSize = pageSize;
    const response = await apiRequest<CursorPagedResponse<T>>(enpdpoint,{ method: "GET", params:finalParams, auth});
    return {
      data: response.data,
      nextCursor: response.nextCursor
    }
  }

  export interface PageParams{
    cursor: string | null
  }