import { FC, createContext, useState, useEffect, ReactNode } from 'react';
import { getUser, loginUser, logoutUser } from './authUtils'; // Import utility functions
import { User, AuthContextType } from './types'

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: FC<{ children: ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        // Check if the user is already logged in on initial load
        const fetchUser = async () => {
            try {
                const currentUser = await getUser();
                setUser(currentUser);
            } catch (error) {
                console.error('Error fetching user:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchUser();
    }, []);

    const login = async (email: string, password: string) => {
        try {
            const currentUser = await loginUser(email, password);
            setUser(currentUser);
        } catch (error) {
            throw new Error('Failed to log in.');
        }
    };

    const logout = async () => {
        try {
            await logoutUser();
            setUser(null);
        } catch (error) {
            console.error('Error logging out:', error);
        }
    };

    return (
        <AuthContext.Provider
          value={{ user, isAuthenticated: !!user, login, logout, loading }}
        >
          {!loading && children}
        </AuthContext.Provider>
      );
    };
    
    export default AuthContext;
