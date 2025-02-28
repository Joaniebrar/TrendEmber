import {  makeCreateRequest } from '@api/queryConfig';

export const runAgent = async (watchlistId : string) => {  
    const params: Record<string, string | number> = {};  
    params['watchList']=watchlistId;
  return makeCreateRequest<string>("runagent",params,true);  
};
