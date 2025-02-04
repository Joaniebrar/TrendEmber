import { useQuery } from "@tanstack/react-query";
import { getTradeSets } from "./api";
import { QUERY_STALE_TIMES,CursorPagedResponse } from "@api/queryConfig"; 
import { TradeSet} from './models'

export const useGetTradeSets = () => {
  return useQuery<CursorPagedResponse<TradeSet>>({
    queryKey: ["tradeSets"],
    queryFn: getTradeSets,
    staleTime: QUERY_STALE_TIMES.SHORT, 
  });
};
