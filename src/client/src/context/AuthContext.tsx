import { createContext, useContext, useState, ReactNode } from "react";
import { useCart } from "./CartContext";
import axios from "axios";
import { baseUrl } from "../Constants/baseUrl";

interface AuthData {
  accessToken: string;
  userId: string;
  userName: string;
  isAdmin: boolean;
}

interface AuthContextType {
  isAuthenticated: boolean;
  isAdmin: boolean;
  user: AuthData | null;
  login: (data: AuthData) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<AuthData | null>(() => {
    const stored = localStorage.getItem("auth");
    return stored ? JSON.parse(stored) : null;
  });

  const { clearCart } = useCart();

  const login = (data: AuthData) => {
    localStorage.setItem("auth", JSON.stringify(data));
    setUser(data);
  };

  const logout = () => {
    localStorage.removeItem("auth");
    localStorage.removeItem("jwt");
    setUser(null);
    clearCart();
    axios.post(
      `${baseUrl}/Utility/ResetSession`,
      {},
      { withCredentials: true }
    );
  };

  const isAuthenticated = !!user?.accessToken;

  return (
    <AuthContext.Provider
      value={{ user, isAuthenticated, isAdmin: !!user?.isAdmin, login, logout }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used inside AuthProvider");
  return context;
};
