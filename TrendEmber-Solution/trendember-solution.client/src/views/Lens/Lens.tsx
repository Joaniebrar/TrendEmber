import { FaChartSimple } from 'react-icons/fa6';
import {useGetTradeSets} from '@api/trades/hooks';

const Lens= () => {
    return (
    <div className="view-content" >
        <h1><FaChartSimple /> Lens</h1>
        <div id='import-tradeset'>
            <button id='import-tradeset'>+ Import</button>
        </div>
        <div id='tradesets-view'>
            <h2>Trade Sets</h2>
            <div className="tradeset-container">
                <div className='trade-set'>
                    <div className="tradeset-item">
                        <span className="tradeset-item-label">Name:</span>
                        <span className="tradeset-item-value">Trade Set 1</span>
                    </div>
                    <div className="tradeset-item">
                        <span className="tradeset-item-label">Imported Date:</span>
                        <span className="tradeset-item-value">2025-02-01</span>
                    </div>
                    <div className="tradeset-item">
                        <span className="tradeset-item-label">Trades (Count):</span>
                        <span className="tradeset-item-value">15</span>
                    </div>
                </div>
            </div>
        </div>
    </div>);
}

export default Lens;