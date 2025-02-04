import {FC, useState, useRef} from 'react';
import {Link, useNavigate} from 'react-router-dom';
import { useAuth } from '@authentication/useAuth';
import useMenuCloseOnClickOutside from '@hooks/useMenuCloseOnClickOutside';
import { navigationItems,loginView } from "../../constants/appRoutes";


const MenuNav: FC = () => {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const menuRef = useRef<HTMLElement>(null);

    const navigate = useNavigate();
    const toggleMenu = () =>{
        setIsMenuOpen(!isMenuOpen);
    };
    useMenuCloseOnClickOutside(menuRef, () => setIsMenuOpen(false), isMenuOpen);
    const { logout, isAuthenticated } = useAuth();
    const handleLogout = () => {
        logout(); 
        setIsMenuOpen(false);
        navigate(loginView.path); 
    };
    return isAuthenticated && (
        <nav className='menu-nav' ref={menuRef}>
            <button className="hamburger" onClick={toggleMenu}>
                <div></div>
                <div></div>
                <div></div>
            </button>                
            <ul className={`menu-nav-list ${isMenuOpen ? 'open' : ''}`}>
                {navigationItems.map((item) => (
                    <li key={item.id} className='menu-nav-list-item'>
                        <Link to={item.path} onClick={item.id === "logout-nav" ? handleLogout : undefined}><item.icon />{item.display}</Link>
                    </li>
                ))}                               
            </ul>
        </nav>          
    );
};
export default MenuNav;
