import { BrowserRouter as Router, useRoutes, Navigate } from "react-router-dom";
import './App.css';
import { AuthProvider } from './authentication/authContext';
import NavBar from './views/ClientNav/NavBar';
import HomePage from './views/Home/Home';
import LoginPage from './views/Login/Login';
import ProtectedRoute from './ProtectedRoute';

const AppRoutes = () => {
    return useRoutes([
      { path: "/login", element: <LoginPage /> },
      {
        element: <ProtectedRoute />, 
        children: [
            { path: "/", element: <HomePage /> },
            { path: "/Home", element: <HomePage /> }, 
            { path: "*", element: <Navigate to="/Home" replace /> }           
        ],
      },
    ]);
  };

function App() {
    return (
        <AuthProvider>
            <Router>
                <NavBar />
                <AppRoutes />
            </Router>        
        </AuthProvider>
    );
    
}

export default App;