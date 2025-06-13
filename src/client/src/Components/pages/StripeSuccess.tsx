import { useNavigate } from "react-router-dom";
import Logo from "../common/Logo";
import { useEffect } from "react";

const StripeSuccess = () => {
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
          <h2 className="text-success mb-3">Payment Successful 🎉</h2>
          <p className="lead">
            Thank you for your purchase! Your order is being prepared and will
            arrive soon.
          </p>
          <p className="text-muted mt-3">
            Please prepare any recyclable items — you can hand them to our
            delivery partner when your products arrive. Together, we're building
            a greener tomorrow. ♻️
          </p>

          <button
            className="btn btn-primary mt-4 px-4 py-2"
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

export default StripeSuccess;
