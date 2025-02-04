import { TradeSet } from "@api/endpoints/trades/models";
import {FC} from 'react';

interface TradeSetItemProps {
    tradeSet: TradeSet
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
            <span className="tradeset-item-value">{tradeSet.importedDate}</span>
        </div>
        <div className="tradeset-item">
            <span className="tradeset-item-label">Trades (Count):</span>
            <span className="tradeset-item-value">{tradeSet.tradeCount}</span>
        </div>
    </div>);   
}

export default TradeSetItem;