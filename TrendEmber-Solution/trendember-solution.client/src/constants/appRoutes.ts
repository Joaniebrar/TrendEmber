import { FaChartLine,FaChartSimple } from 'react-icons/fa6';
import { FaHome,FaCloudDownloadAlt } from "react-icons/fa"; 
import { GiElectricalResistance, GiArtificialIntelligence } from "react-icons/gi";
import { IoMdLogOut } from "react-icons/io";
import { PiStrategy } from "react-icons/pi";
import { IconType } from "react-icons";

export interface NavigationItem {
    id: string;
    display: string;
    icon: IconType;
    path: string;
  }

export const navigationItems: NavigationItem[] = [
  { id: "home-nav", display: 'Home', icon: FaHome , path: '/' },
  {  id: "lens-nav",display: 'Lens', icon: FaChartSimple, path: '/Lens' },
  {  id: "harvest-nav",display: 'Harvester', icon: FaCloudDownloadAlt, path: '/Harvester' },
  {  id: "threshold-nav",display: 'Threshold', icon: GiElectricalResistance, path: '/Threshold' },
  {  id: "visualizer-nav",display: 'Visualizer', icon: FaChartLine , path: '/Visualizer' },
  {  id: "stratium-nav",display: 'Stratum', icon: PiStrategy , path: '/Stratum' },
  {  id: "patterniq-nav",display: 'PatternIQ', icon: GiArtificialIntelligence , path: '/PatternIQ' },
  {  id: "logout-nav",display: 'Logout', icon: IoMdLogOut, path: '/Logout' },
];

export default navigationItems;
