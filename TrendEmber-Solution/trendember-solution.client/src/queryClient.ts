import { QueryClient } from "@tanstack/react-query";
import { QUERY_STALE_TIMES } from "@api/queryConfig";

export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: QUERY_STALE_TIMES.SHORT, 
    },
  },
});
