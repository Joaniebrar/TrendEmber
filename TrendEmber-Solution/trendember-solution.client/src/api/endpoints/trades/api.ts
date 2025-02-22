import { makePagedRequest, makeCreateRequest, PageParams } from '@api/queryConfig';
import { TradeSet } from './models';

export const getTradeSets = async ({ cursor }: PageParams) => {  
  const params: Record<string, string | number> = {};  
  return makePagedRequest<TradeSet>("tradesets",params,cursor,10);  
};

export const importTradeSets = async ({ file, name, mapping, ignoreFirstRow }: ImportTradeSetsParams) => {  
  const formData = new FormData();
  formData.append("file", file);
  formData.append("name", name);
  formData.append("mapping", mapping);
  formData.append("ignoreFirstRow", ignoreFirstRow.toString());
  console.log(JSON.stringify(formData));
  return makeCreateRequest<TradeSet>("tradesets",formData,true);  
};

export interface ImportTradeSetsParams{
  file: File, 
  name: string, 
  mapping: string, 
  ignoreFirstRow: boolean
}