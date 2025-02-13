import { TradeSet } from "@api/endpoints/trades/models";
import {FC} from 'react';

interface TradeSetItemProps {
    tradeSet: TradeSet
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

const TradeSetItem: FC<TradeSetItemProps>  = ({tradeSet}) => {
    return (
    <div className='trade-set'>
        <div className="tradeset-item">
            <span className="tradeset-item-label">Name:</span>
            <span className="tradeset-item-value">{tradeSet.name}</span>
        </div>
        <div className="tradeset-item">
            <span className="tradeset-item-label">Imported Date:</span>
            <span className="tradeset-item-value">{convertToLocalDate(tradeSet.importedDate)}</span>
        </div>
        <div className="tradeset-item">
            <span className="tradeset-item-label">Trades (Count):</span>
            <span className="tradeset-item-value">{tradeSet.tradeCount}</span>
        </div>
    </div>);   
}

export default TradeSetItem;