import { WatchList } from "@api/endpoints/watchList/models";
import {FC} from 'react';
import { FaCircle } from "react-icons/fa";
import { useNavigate } from "react-router-dom";
import {WatchListView} from '../../constants/appRoutes';

interface WatchListItemItemProps {
    watchList: WatchList
}


function convertToLocalDate(isoDateString: string) {
    if (!isoDateString) return null;

    const date = new Date(isoDateString);
    return date.toLocaleDateString("en-US", {  
        year: "numeric",  
        month: "long", 
        day: "numeric"  
    });
}

const statusColors: Record<string, string> = {
    Stopped: "agent-status-red",
    Running: "agent-status-green",
  };


const WatchListItem: FC<WatchListItemItemProps>  = ({watchList}) => {
    const navigate = useNavigate();
    const handleClick = () => {
        const pathWithId = WatchListView.path.replace(':id', watchList.name);
        navigate(pathWithId, { state: { watchList } });

      };

    return (
    <div  onClick={handleClick}  className='watchlist-set'>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Name:</span>
            <span className="watchlist-item-value">{watchList.name}</span>
        </div>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Imported Date:</span>
            <span className="watchlist-item-value">{convertToLocalDate(watchList.importedDate)}</span>
        </div>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Symbol (Count):</span>
            <span className="watchlist-item-value">{watchList.symbolCount}</span>
        </div>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Agent Status:</span>
            <span className="watchlist-item-value"><FaCircle className={`${statusColors[watchList.agentStatus]}`} /></span>
        </div>        
    </div>);   
}

export default WatchListItem;