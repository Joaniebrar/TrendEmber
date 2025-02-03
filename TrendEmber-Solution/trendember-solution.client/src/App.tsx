import { BrowserRouter as Router, useRoutes, Navigate } from "react-router-dom";
import './App.css';
import { AuthProvider } from './authentication/authContext';
import NavBar from './views/ClientNav/NavBar';
import ProtectedRoute from './ProtectedRoute';
import {loginView,routableViews} from './constants/appRoutes';

const AppRoutes = () => {
    return useRoutes([
      { path: loginView.path, element: <loginView.view />},
      {
        element: <ProtectedRoute />, 
        children: [
            ...routableViews.map(item=>({
              path:item.path,element: <item.view />
            })),
            { path: "*", element: <Navigate to="/" replace /> }           
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