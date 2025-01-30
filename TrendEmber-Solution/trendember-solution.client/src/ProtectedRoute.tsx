import { FC, useContext } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { AuthContext } from './authentication/authContext';

const ProtectedRoute: FC = () => {
  const { isAuthenticated, isLoading } = useContext(AuthContext);

  if (isLoading)
  {
    return;
  }
  return isAuthenticated ? <Outlet /> : <Navigate to="/login" replace />;
};

export default ProtectedRoute;
