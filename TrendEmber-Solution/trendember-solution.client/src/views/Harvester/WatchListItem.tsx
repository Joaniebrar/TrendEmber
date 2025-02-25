import { WatchList } from "@api/endpoints/watchList/models";
import {FC} from 'react';

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

const WatchListItem: FC<WatchListItemItemProps>  = ({watchList}) => {
    return (
    <div className='watchlist-set'>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Name:</span>
            <span className="watchlist-item-value">{watchList.name}</span>
        </div>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Imported Date:</span>
            <span className="watchlist-item-value">{convertToLocalDate(watchList.importedDate)}</span>
        </div>
        <div className="watchlist-item">
            <span className="watchlist-item-label">Trades (Count):</span>
            <span className="watchlist-item-value">{watchList.symbolCount}</span>
        </div>
    </div>);   
}

export default WatchListItem;