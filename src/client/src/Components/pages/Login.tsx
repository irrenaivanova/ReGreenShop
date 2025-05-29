import { useNavigate } from "react-router-dom";
import { authService } from "../../services/authService";
import Logo from "../common/Logo";
import { useForm } from "react-hook-form";
import { useAuth } from "../../context/AuthContext";
import { useModal } from "../../context/ModalContext";

interface LoginFormInputs {
  email: string;
  password: string;
}

const Login = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormInputs>();
  const { login } = useAuth();
  const { showModal } = useModal();
  const navigate = useNavigate();

  const onSubmit = async (data: LoginFormInputs) => {
    try {
      const response = await authService.login(data.email, data.password);
      const { accessToken, userId, userName, isAdmin } = response.data.data;
      const successMessage = response.data.message || "Login successful!";

      login({ accessToken, userId, userName, isAdmin });
      showModal?.("success", successMessage);
      navigate("/cart");
    } catch (error: any) {
      const errorMessage = error.response?.data?.error || "Login failed.";
      showModal?.("error", errorMessage);
    }
  };

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
        <div style={{ flex: 1, padding: "50px" }}>
          <h2 className="text-center mb-4">Login</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="mb-4">
              <label>Email</label>
              <input
                type="email"
                className={`form-control ${errors.email ? "is-invalid" : ""}`}
                {...register("email", { required: "Email is required" })}
              />
              {errors.email && (
                <div className="invalid-feedback">{errors.email.message}</div>
              )}
            </div>

            <div className="mb-4">
              <label>Password</label>
              <input
                type="password"
                className={`form-control ${
                  errors.password ? "is-invalid" : ""
                }`}
                {...register("password", { required: "Password is required" })}
              />
              {errors.password && (
                <div className="invalid-feedback">
                  {errors.password.message}
                </div>
              )}
            </div>

            <button type="submit" className="btn btn-primary small w-100 py-2">
              Login
            </button>
          </form>

          <div className="text-center mt-5">
            <Logo
              style={{ height: "80px", width: "auto", marginBottom: "10px" }}
            />
            <div className="fw-bold text-primary mt-2">
              Shop Smart. Live Green.
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
