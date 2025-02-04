import { makePagedRequest } from '@api/queryConfig';
import { TradeSet } from './models';

export const getTradeSets = async () => {  
  return makePagedRequest<TradeSet>("tradesets");  
};