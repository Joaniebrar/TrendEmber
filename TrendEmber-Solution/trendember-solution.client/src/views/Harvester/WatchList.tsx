import { useNavigate } from "react-router-dom";
import {HarvesterView} from '../../constants/appRoutes';
import { IoChevronBackCircleSharp } from "react-icons/io5";
import { useLocation } from "react-router-dom";
import { WatchList } from "@api/endpoints/watchList/models";

const WatchListPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const watchList = location.state?.watchList as WatchList;
    return <div>
        <div role="button" tabIndex={0} style={{ cursor: "pointer" }} 
            onClick={()=>{navigate(HarvesterView.path);}}><IoChevronBackCircleSharp /> Back to Harvester</div>
            <h2>Watch list {watchList.name}</h2>
        </div>
}

export default WatchListPage;