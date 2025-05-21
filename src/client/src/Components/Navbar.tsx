// components/Navbar.tsx
import { Link, useLocation } from "react-router-dom";

const Navbar = () => {
  const location = useLocation();

  return (
    <nav>
      {/* Normal nav links */}
      <Link to="/" className="nav-link">
        Home
      </Link>

      {/* Modal route links */}
      <Link
        to="/login"
        state={{ backgroundLocation: location }}
        className="nav-link"
      >
        Login
      </Link>

      <Link
        to="/register"
        state={{ backgroundLocation: location }}
        className="nav-link"
      >
        Register
      </Link>
    </nav>
  );
};

export default Navbar;
