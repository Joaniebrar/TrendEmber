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
            <div className="header-content">
                <span className="glasses-icon">
                    <svg width="30" height="25" viewBox="0 0 64 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <rect x="8" y="18" width="20" height="12" rx="4" stroke="white" stroke-width="2" fill="none"/>
                        <rect x="36" y="18" width="20" height="12" rx="4" stroke="white" stroke-width="2" fill="none"/>
                        <line x1="28" y1="24" x2="36" y2="24" stroke="white" stroke-width="2"/> 
                        <polyline points="12,26 16,22 20,25 24,20" stroke="blue" stroke-width="2" fill="none"/>
                        <polyline points="40,22 44,26 48,21 52,24" stroke="green" stroke-width="2" fill="none"/>        
                    </svg>
                </span>
                <h1>TrendEmber</h1>
            </div>
            <nav className='menu-nav' ref={menuRef}>
                <button className="hamburger" onClick={toggleMenu}>
                <div></div>
                    <div></div>
                    <div></div>
                </button>                
            </nav>            
        </header>
        );
}

export default NavBar;