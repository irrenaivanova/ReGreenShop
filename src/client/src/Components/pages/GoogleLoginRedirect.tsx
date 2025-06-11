import { useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";
import { useModal } from "../../context/ModalContext";

const GoogleLoginRedirect = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { login } = useAuth();
  const { showModal } = useModal();

  useEffect(() => {
    const params = new URLSearchParams(location.search);

    const accessToken = params.get("accessToken");
    const userId = params.get("userId");
    const userName = params.get("userName");
    const isAdmin = params.get("isAdmin") === "true";

    if (accessToken && userId && userName) {
      login({ accessToken, userId, userName, isAdmin });
      showModal?.(
        "success",
        `Welcome, ${userName} Login with Google successful!`
      );
      navigate("/cart");
    } else {
      showModal?.("error", "Google login failed.");
      navigate("/login");
    }
  }, []);

  return <div>Logging you in...</div>;
};

export default GoogleLoginRedirect;
