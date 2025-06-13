import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import Logo from "../common/Logo";

const StripeFail = () => {
  const navigate = useNavigate();

  // Auto-redirect after 10 seconds
  useEffect(() => {
    const timeout = setTimeout(() => {
      navigate("/");
    }, 10000);
    return () => clearTimeout(timeout);
  }, [navigate]);

  return (
    <div
      className="d-flex justify-content-center align-items-center vh-100"
      style={{ padding: "20px" }}
    >
      <div
        className="card shadow-lg d-flex flex-row"
        style={{ maxWidth: "900px", width: "100%", minHeight: "500px" }}
      >
        <div
          className="d-none d-md-block login-side-image"
          style={{ flex: 1.5 }}
        ></div>

        <div
          className="d-flex flex-column align-items-center justify-content-center text-center bg-white"
          style={{ flex: 1, padding: "50px" }}
        >
          <h2 className="text-warning mb-3">Payment Not Completed ⚠️</h2>
          <p className="lead">
            Your order has been placed successfully, but the payment was not
            completed online.
          </p>
          <p className="text-muted mt-3">
            Please have your payment ready — our delivery partner will collect
            it when your order arrives. Thank you for shopping with us!
          </p>

          <button
            className="btn btn-outline-primary mt-4 px-4 py-2"
            onClick={() => navigate("/")}
          >
            Go to Homepage
          </button>

          <div className="text-center mt-5">
            <Logo
              style={{ height: "80px", width: "auto", marginBottom: "10px" }}
            />
            <div className="fw-bold text-primary mt-3">
              Shop Smart. Live Green.
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default StripeFail;
