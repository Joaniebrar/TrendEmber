import {FC, useState, useRef, useEffect} from 'react';
import {Link} from 'react-router-dom';
import './NavBar.css'

const NavBar: FC = () => {
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const menuRef = useRef<HTMLDivElement>(null);
    const toggleMenu = () =>{
        setIsMenuOpen(!isMenuOpen);
    };
    const handleClickOutside = (event: MouseEvent) => {
        if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
            setIsMenuOpen(false);
        }
    };
    useEffect(() => {
        if (isMenuOpen) {
            document.addEventListener("mousedown", handleClickOutside);
        } else {
            document.removeEventListener("mousedown", handleClickOutside);
        }

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [isMenuOpen]);
    return (
        <header className="header">
             <h1>TrendEmber</h1>
    <nav className='menu-nav' ref={menuRef}>
                    <button className="hamburger" onClick={toggleMenu}>
                <div className="hamburger-icon">
                    <div></div>
                    <div></div>
                    <div></div>
                </div>
            </button>
        <ul className={`menu-nav-list ${isMenuOpen ? 'open' : ''}`}>
            <li className='menu-nav-list-item'><Link to="/">Home</Link></li>
            <li className='menu-nav-list-item'><Link to="/Lens">Lens</Link></li>
            <li className='menu-nav-list-item'><Link to="/Harvester">Harvester</Link></li>
            <li className='menu-nav-list-item'><Link to="/Threshold">Threshold</Link></li>
            <li className='menu-nav-list-item'><Link to="/Visualizer">Visualizer</Link></li>
            <li className='menu-nav-list-item'><Link to="/Stratum">Stratum</Link></li>
            <li className='menu-nav-list-item'><Link to="/PatternIQ">PatternIQ</Link></li>            
        </ul>
    </nav>
    </header>);
}

export default NavBar;