import { useNavigate } from "react-router-dom";
import {HarvesterView} from '../../constants/appRoutes';
import { IoChevronBackCircleSharp } from "react-icons/io5";
import { useLocation } from "react-router-dom";
import { WatchList } from "@api/endpoints/watchList/models";
import {useGetWatchListSymbols} from '@api/endpoints/watchList/hooks';
import {useRunAgent} from '@api/endpoints/runAgent/hooks';


const WatchListPage = () => {
    const navigate = useNavigate();
    const location = useLocation();    
    const watchList = location.state?.watchList as WatchList;
    const { data, error, isLoading } = useGetWatchListSymbols(watchList.id); 
    const { mutate: runAgent } = useRunAgent();
    return (
    <div className="watchListContent">
        <div className="watchlist-container-header"> 
            <div role="button" tabIndex={0} style={{ cursor: "pointer" }} 
                onClick={()=>{navigate(HarvesterView.path);}}><IoChevronBackCircleSharp /> Back to Harvester</div>    
            <div className="watchlist-agent-header-row">
                <h2>Watch list {watchList.name}</h2>
                <button id="run-agent" onClick={()=> runAgent(watchList.id)}>Run Agent</button>
            </div>
        </div>                    
            <table>
                <thead>
                    <tr>
                    <th>Symbol</th>
                    <th>Market</th>
                    <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                {data && data.data?.map((item) => (
                    <tr key={item.symbol}>
                        <td>{item.symbol}</td>
                        <td>{item.market}</td>
                        <td>$6.50</td>
                    </tr>
                ))}
      </tbody>
    </table>
        </div>);
}

export default WatchListPage;