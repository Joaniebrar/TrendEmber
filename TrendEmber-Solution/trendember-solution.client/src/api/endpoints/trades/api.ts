import { makePagedRequest, PageParams } from '@api/queryConfig';
import { TradeSet } from './models';

export const getTradeSets = async ({ cursor }: PageParams) => {  
  const params: Record<string, string | number> = {};  
  return makePagedRequest<TradeSet>("tradesets",params,cursor,10);  
};