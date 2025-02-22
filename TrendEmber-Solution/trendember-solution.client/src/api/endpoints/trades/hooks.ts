import { useInfiniteQuery, useMutation,useQueryClient  } from "@tanstack/react-query";
import { getTradeSets, importTradeSets, ImportTradeSetsParams } from "./api";
import { QUERY_STALE_TIMES,CursorPagedResponse } from "@api/queryConfig"; 
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

export const useImportTradeSets = () => {
  const queryClient = useQueryClient();

  return useMutation<TradeSet, Error, ImportTradeSetsParams>({
    mutationFn: importTradeSets,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["tradeSets"] });
    },
    onError: (error) => {
      console.error("Import failed:", error);
    },
  });
};
