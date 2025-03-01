import { makeGetRequest} from '@api/queryConfig';
import { TrainerData } from './models';

export const getTrainerSample = async () => {  
  const params: Record<string, string | number> = {};  
  return makeGetRequest<TrainerData>("trainer",params,true);  
};

