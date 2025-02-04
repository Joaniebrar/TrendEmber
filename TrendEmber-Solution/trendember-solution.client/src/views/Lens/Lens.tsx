import { FaChartSimple } from 'react-icons/fa6';
import {useGetTradeSets} from '@api/endpoints/trades/hooks';
import TradeSetItem  from './TradeSetItem';

const Lens= () => {
    const { data, error, isLoading } = useGetTradeSets();

    if (isLoading) return <p>Loading...</p>;
    if (error) return <p>Error fetching trade sets</p>;

    return (
    <div className="view-content" >
        <h1><FaChartSimple /> Lens</h1>
        <div id='import-tradeset'>
            <button id='import-tradeset'>+ Import</button>
        </div>
        <div id='tradesets-view'>
            <h2>Trade Sets</h2>
            <div className="tradeset-container">                            
                {data && 
                    <div className='trade-set'>
                    {data.data.map((item) => (<TradeSetItem key ={item.id} tradeSet= {item}/>))}
                    </div>
                } 
            </div>
        </div>
        <div>
    </div>
    </div>);
}

export default Lens;
      