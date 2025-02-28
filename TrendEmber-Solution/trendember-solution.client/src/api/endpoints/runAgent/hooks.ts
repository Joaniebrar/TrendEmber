import { useMutation   } from "@tanstack/react-query";
import { runAgent } from "./api";

export const useRunAgent = () => {
    return useMutation({
      mutationFn: (watchlistId: string) => runAgent(watchlistId),
    });
  };

