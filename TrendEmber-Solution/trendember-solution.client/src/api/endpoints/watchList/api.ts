import { makePagedRequest, makeCreateRequest, PageParams, makeGetRequest} from '@api/queryConfig';
import { WatchList, WatchListSymbol } from './models';

export const getWatchLists = async ({ cursor }: PageParams) => {  
  const params: Record<string, string | number> = {};  
  return makePagedRequest<WatchList>("watchlists",params,cursor,10);  
};

export const importWatchLists = async ({ file, name, mapping, ignoreFirstRow }: ImportWatchListsParams) => {  
  const formData = new FormData();
  formData.append("file", file);
  formData.append("name", name);
  formData.append("mapping", mapping);
  formData.append("ignoreFirstRow", ignoreFirstRow.toString());
  return makeCreateRequest<WatchList>("watchlists",formData,true);  
};

export interface ImportWatchListsParams{
  file: File, 
  name: string, 
  mapping: string, 
  ignoreFirstRow: boolean
}

export const getWatchListSymbols = async (watchlistId : string) => {  
  const params: Record<string, string | number> = {};  
  params['watchListId']=watchlistId;
  return makeGetRequest<WatchListSymbol>("watchlistsymbols",params,true);  
};