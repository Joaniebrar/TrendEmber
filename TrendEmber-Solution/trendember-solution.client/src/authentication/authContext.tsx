import { createContext, useState, ReactNode, useContext, useEffect } from "react";
import { loginUser } from './authUtils';
import { logoutUser } from './authUtils';
import {User} from './types';

interface AuthContextType {
  user: User | null;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<boolean>;
  logout: () => void;
  error: string | null;
  isLoading: boolean
}

export const AuthContext = createContext<AuthContextType>({} as AuthContextType);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [user, setUser] = useState<User | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [isLoading,setLoading] = useState<boolean>(true);
    useEffect(() => {
        const token = localStorage.getItem('authToken');
        if (token)
        {
            setIsAuthenticated(true);
        }
        setLoading(false);
    }, []);
    const login = async (email: string, password: string): Promise<boolean> => {
        try {
            const token = await loginUser(email,password);            
            if (token) {
                localStorage.setItem('authToken',token);
                setIsAuthenticated(true);
                setUser({email} as User);
                setError(null);
                return true;
            }
            return false;
        }
        catch(err) {
            setError("Login failed. Please try again.");
            setIsAuthenticated(false);
            setUser(null);
            return false;
        }
    };

    const logout = async ()=> {
        await logoutUser();
        localStorage.removeItem('authToken'); 
        setError(null);
        setUser(null);
        setIsAuthenticated(false);
    };
    

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, error,login, logout, isLoading}}>
      {children}
    </AuthContext.Provider>
  );
};
