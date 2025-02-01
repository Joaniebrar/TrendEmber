import {FC} from 'react';
import './NavBar.css'

import GlassesIcon from './GlassesIcon';
import MenuNav from './MenuNav';

const NavBar: FC = () => {

    return (        
        <header className="header">
            <div className="header-content">
                <GlassesIcon />
                <h1>TrendEmber</h1>
            </div>
            <MenuNav />
        </header>
        );
}

export default NavBar;