import { useInfiniteQuery, useMutation,useQueryClient  } from "@tanstack/react-query";
import { getWatchLists, importWatchLists, ImportWatchListsParams } from "./api";
import { QUERY_STALE_TIMES,CursorPagedResponse } from "@api/queryConfig"; 
import { WatchList} from './models'

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
