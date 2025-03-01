import { useQuery   } from "@tanstack/react-query";
import { getTrainerSample } from "./api";
import { GetResponse } from "@api/queryConfig"; 
import { TrainerData} from './models'

export const useGetTrainerSample = (enabled: boolean) => {
  return useQuery<GetResponse<TrainerData>, Error>({
    queryKey: ["gettrainersample"],     
    queryFn: () => getTrainerSample(),
    staleTime: 0,     
    refetchOnWindowFocus: true, 
    refetchOnReconnect: true, 
    enabled: enabled
  });
};
