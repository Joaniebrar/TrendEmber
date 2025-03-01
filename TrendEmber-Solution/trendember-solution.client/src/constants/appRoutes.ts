import { /*FaChartLine,*/FaChartSimple } from 'react-icons/fa6';
import { FaHome,FaCloudDownloadAlt } from "react-icons/fa"; 
//import { GiElectricalResistance, GiArtificialIntelligence } from "react-icons/gi";
import { IoMdLogOut } from "react-icons/io";
//import { PiStrategy } from "react-icons/pi";
import { IconType } from "react-icons";
import HomePage from '../views/Home/Home';
import LensPage from '../views/Lens/Lens';
import HarvesterPage from '../views/Harvester/Harvester';
import WatchListPage from '../views/Harvester/WatchList';
import LoginPage from '../views/Login/Login';
import TrainerPage from '../views/Trainer/Trainer';

export interface ViewItem {
    id: string;
    view: React.ComponentType<any>;
    path: string;
    menuItem: boolean
}
export interface NavigationItem extends ViewItem{
    icon: IconType;
    display: string;
  }

const homeId = "home-nav";
const lensId = "lens-nav";
const harvesterId = "harvest-nav";
const loginId = "login-nav";
const logoutId = "logout-nav";
const watchListId = "watchlist-view";
const trainerId = "trainer-view";

const navigationMetadata: Record<string, {icon: IconType, display:string}>= {
    [homeId]: { display: 'Home', icon: FaHome},
    [lensId]: { display: 'Lens', icon: FaChartSimple},
    [harvesterId]: { display: 'Harvester', icon: FaCloudDownloadAlt},
    [logoutId]: { display: 'Logout', icon: IoMdLogOut },    
};

const viewItems: ViewItem[] = [
    {id: homeId,view: HomePage, path:'/', menuItem:true},
    {id: lensId,view: LensPage, path:'/Lens', menuItem:true},
    {id: harvesterId,view: HarvesterPage, path:'/Harvester', menuItem:true},
    {id: watchListId,view: WatchListPage, path:'/watchlist/:id', menuItem:false},
    {id: loginId,view: LoginPage, path:'/Login', menuItem:false},
    {id: logoutId, view: HomePage, path:'/Logout', menuItem:true},
    {id: trainerId,view: TrainerPage, path:'/Trainer', menuItem:false},
];
export const navigationItems: NavigationItem[] = 
    viewItems
        .filter(item => item.menuItem)
        .map(item =>({
            ...item,
            ...navigationMetadata[item.id],
        }));
export const loginView = viewItems.find(item=>item.id==loginId) as ViewItem;
export const routableViews = viewItems.filter(item=>item.id !==loginId && item.id !==logoutId);
export const WatchListView = viewItems.find(item=>item.id==watchListId) as ViewItem;
export const HarvesterView = viewItems.find(item=>item.id==harvesterId) as ViewItem;
/*
    { id: "home-nav", display: 'Home', icon: FaHome , path: '/' },
    {  id: "lens-nav",display: 'Lens', icon: FaChartSimple, path: '/Lens' },
    {  id: "harvest-nav",display: 'Harvester', icon: FaCloudDownloadAlt, path: '/Harvester' },
    {  id: "threshold-nav",display: 'Threshold', icon: GiElectricalResistance, path: '/Threshold' },
    {  id: "visualizer-nav",display: 'Visualizer', icon: FaChartLine , path: '/Visualizer' },
    {  id: "stratium-nav",display: 'Stratum', icon: PiStrategy , path: '/Stratum' },
    {  id: "patterniq-nav",display: 'PatternIQ', icon: GiArtificialIntelligence , path: '/PatternIQ' },
    {  id: "logout-nav",display: 'Logout', icon: IoMdLogOut, path: '/Logout' },*/

    /*harvestId: {display: 'Harvester', icon: FaCloudDownloadAlt },
    thresholdId: {display: 'Threshold', icon: GiElectricalResistance },
    visualizerId: {display: 'Visualizer', icon: FaChartLine },
    stratiumId: {display: 'Stratum', icon: PiStrategy },
    patternIQId: {display: 'PatternIQ', icon: GiArtificialIntelligence },*/    

    /*const harvestId = "harvest-nav";
const thresholdId = "threshold-nav";
const visualizerId = "visualizer-nav";
const stratiumId = "stratium-nav";
const patternIQId = "patterniq-nav";*/