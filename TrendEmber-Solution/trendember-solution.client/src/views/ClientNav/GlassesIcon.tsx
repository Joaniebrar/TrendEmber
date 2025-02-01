import {FC} from 'react';

interface GlassesIconProps {
    width?: number;
    height?: number;
    className?: string;
}

const GlassesIcon: FC<GlassesIconProps> = ({width = 30, height = 25, className=''}) => {
    return (
        <span className={`glasses-icon ${className}`}>
            <svg width={width} height={height} viewBox="0 0 64 48" fill="none" xmlns="http://www.w3.org/2000/svg">
                <rect x="8" y="18" width="20" height="12" rx="4" stroke="white" strokeWidth="2" fill="none"/>
                <rect x="36" y="18" width="20" height="12" rx="4" stroke="white" strokeWidth="2" fill="none"/>
                <line x1="28" y1="24" x2="36" y2="24" stroke="white" strokeWidth="2"/> 
                <polyline points="12,26 16,22 20,25 24,20" stroke="blue" strokeWidth="2" fill="none"/>
                <polyline points="40,22 44,26 48,21 52,24" stroke="green" strokeWidth="2" fill="none"/>        
            </svg>            
        </span>        
    );
};

export default GlassesIcon;