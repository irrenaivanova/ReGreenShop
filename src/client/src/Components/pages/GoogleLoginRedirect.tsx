import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { useModal } from "../../context/ModalContext";
import axios from "axios";
import { baseUrl } from "../../Constants/baseUrl";

const GoogleLoginRedirect = () => {
  const { login } = useAuth();
  const { showModal } = useModal();
  const navigate = useNavigate();

  useEffect(() => {
    const verifyLogin = async () => {
      try {
        const response = await axios.get(`${baseUrl}/User/Profile`, {
          withCredentials: true,
        });
        const { accessToken, userId, userName, isAdmin } = response.data.data;
        login({ accessToken, userId, userName, isAdmin });
        showModal?.("success", "Login with Google successful!");
        navigate("/cart");
      } catch {
        showModal?.("error", "Google login failed.");
        navigate("/login");
      }
    };

    verifyLogin();
  }, []);

  return <div className="text-center mt-5">Logging you in with Google...</div>;
};

export default GoogleLoginRedirect;
