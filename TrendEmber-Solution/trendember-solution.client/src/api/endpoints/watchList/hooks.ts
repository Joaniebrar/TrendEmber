import { useInfiniteQuery, useMutation,useQueryClient, useQuery   } from "@tanstack/react-query";
import { getWatchLists, importWatchLists, ImportWatchListsParams, getWatchListSymbols } from "./api";
import { QUERY_STALE_TIMES,CursorPagedResponse, GetResponse } from "@api/queryConfig"; 
import { WatchList, WatchListSymbol} from './models'

export const useGetWatchLists = () => {
  return useInfiniteQuery<CursorPagedResponse<WatchList>>({
    queryKey: ["watchlists"],
    initialPageParam: null,
    
    queryFn: ({ pageParam = null }) => {
      const cursorDate = pageParam ? new Date(String(pageParam)) : null;
      return getWatchLists({cursor: cursorDate?.toISOString() || null});
    },
    getNextPageParam: (lastPage) => lastPage?.nextCursor || null,
    staleTime: QUERY_STALE_TIMES.SHORT, 
  });
};

export const useImportWatchLists = () => {
  const queryClient = useQueryClient();

  return useMutation<WatchList, Error, ImportWatchListsParams>({
    mutationFn: importWatchLists,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["watchlists"] });
    },
    onError: (error) => {
      console.error("Import failed:", error);
    },
  });
};

export const useGetWatchListSymbols = (watchlistId: string) => {
  return useQuery<GetResponse<WatchListSymbol>, Error>({
    queryKey: ["watchlistsymbols", watchlistId], 
    queryFn: () => getWatchListSymbols(watchlistId),
    enabled: !!watchlistId, 
  });
};
