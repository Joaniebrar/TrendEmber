import { useEffect, RefObject } from "react";


function useMenuCloseOnClickOutside<T extends HTMLElement>(
    ref: RefObject<T>,
    callback: () => void,
    isActive: boolean = true
  ) {
    useEffect(() => {
      if (!isActive) return;
  
      const handleClickOutside = (event: MouseEvent) => {
        if (ref.current && !ref.current.contains(event.target as Node)) {
          callback();
        }
      };
  
      document.addEventListener("mousedown", handleClickOutside);
      return () => {
        document.removeEventListener("mousedown", handleClickOutside);
      };
    }, [ref, callback, isActive]);
  }
  
  export default useMenuCloseOnClickOutside;