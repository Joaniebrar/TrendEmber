// Represents a user object
export interface User {    
    fullName: string;
    email: string;
    roles: string[];    
  }



  // Type for the authentication context
  export interface AuthContextType {
    user: User | null;
    isAuthenticated: boolean;
    login: (email: string, password: string) => Promise<void>;
    logout: () => Promise<void>;
    loading: boolean;
  }
  
  // Additional types related to API requests/responses
  export interface LoginResponse {
    user: User;
    token: string; // JWT or any other token
  }
  
  export interface ErrorResponse {
    message: string;
    statusCode: number;
  }
  
  // Define roles if your application uses role-based access
  export type UserRole = 'admin' | 'analyst' | 'guest';
  