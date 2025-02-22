import { useInfiniteQuery } from "@tanstack/react-query";
import { getTradeSets } from "./api";
import { QUERY_STALE_TIMES,CursorPagedResponse, PageParams } from "@api/queryConfig"; 
import { TradeSet} from './models'

export const useGetTradeSets = () => {
  return useInfiniteQuery<CursorPagedResponse<TradeSet>>({
    queryKey: ["tradeSets"],
    initialPageParam: null,
    
    queryFn: ({ pageParam = null }) => {
      const cursorDate = pageParam ? new Date(String(pageParam)) : null;
      return getTradeSets({cursor: cursorDate?.toISOString() || null});
    },
    getNextPageParam: (lastPage) => lastPage?.nextCursor || null,
    staleTime: QUERY_STALE_TIMES.SHORT, 
  });
};
