import { useForm } from "react-hook-form";
import { useAuth } from "../context/AuthContext";
import { useModal } from "../context/ModalContext";
import { authService } from "../services/authService";
import { useNavigate } from "react-router-dom";
import Logo from "../Components/Logo";
import "../App.css";

interface RegisterFormInputs {
  userName: string;
  password: string;
}

const Register = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormInputs>();
  const { login } = useAuth();
  const { showModal } = useModal();
  const navigate = useNavigate();

  const onSubmit = async (data: RegisterFormInputs) => {
    try {
      const response = await authService.register(data.userName, data.password);
      const { accessToken, userId, userName, isAdmin } = response.data.data;
      const successMessage =
        response.data.message || "Registration successful!";

      // Automatically login user after registration
      login({ accessToken, userId, userName, isAdmin });
      showModal?.("success", successMessage);
      navigate("/");
    } catch (error: any) {
      const errorMessage =
        error.response?.data?.error || "Registration failed.";
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
          <h2 className="text-center mb-4">Register</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="mb-4">
              <label>Username</label>
              <input
                type="text"
                className={`form-control ${
                  errors.userName ? "is-invalid" : ""
                }`}
                {...register("userName", { required: "Username is required" })}
              />
              {errors.userName && (
                <div className="invalid-feedback">
                  {errors.userName.message}
                </div>
              )}
            </div>

            <div className="mb-4">
              <label>Password</label>
              <input
                type="password"
                className={`form-control ${
                  errors.password ? "is-invalid" : ""
                }`}
                {...register("password", {
                  required: "Password is required",
                  minLength: {
                    value: 6,
                    message: "Password must be at least 6 characters",
                  },
                })}
              />
              {errors.password && (
                <div className="invalid-feedback">
                  {errors.password.message}
                </div>
              )}
            </div>

            <button type="submit" className="btn btn-primary small w-100 py-2">
              Register
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

export default Register;
