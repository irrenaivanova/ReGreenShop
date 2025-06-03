import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { FaList, FaShoppingCart, FaSearch } from "react-icons/fa";
import { Dropdown, Form, InputGroup, Button, Badge } from "react-bootstrap";
import { categoriesService } from "../../services/categoriesService";
import { RootCategory } from "../../types/RootCategory";
import { baseUrl } from "../../Constants/baseUrl";
import { useAuth } from "../../context/AuthContext";
import { useCart } from "../../context/CartContext";
import DOMPurify from "dompurify";

const Header = () => {
  const [rootCategories, setRootCategories] = useState<RootCategory[]>([]);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const { isAuthenticated, logout } = useAuth();
  const { cartCount, loading, refreshCartCount } = useCart();
  const navigate = useNavigate();

  useEffect(() => {
    categoriesService
      .getRootCategories()
      .then((res) => setRootCategories(res.data.data))
      .catch(() => setRootCategories([]));
  }, []);

  // Refresh cart count when auth status changes
  useEffect(() => {
    refreshCartCount();
  }, [isAuthenticated, refreshCartCount]);

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault();
    if (!searchTerm.trim()) return;
    navigate(
      `/search?query=${encodeURIComponent(
        DOMPurify.sanitize(searchTerm.trim())
      )}`
    );
  };

  const handleLogout = () => {
    logout();
    navigate("/");
  };

  return (
    <header className="bg-primary text-white">
      <div className="container py-3 d-flex flex-wrap align-items-center justify-content-between">
        <Link to="/" className="text-white text-decoration-none">
          <h3 className="mb-0 me-4">ReGreenShop</h3>
        </Link>

        <Dropdown>
          <Dropdown.Toggle
            variant="outline-light"
            size="lg"
            id="categoriesDropdown"
            className="d-flex align-items-center"
          >
            <FaList className="me-2 text-warning" />
            Categories
          </Dropdown.Toggle>
          <Dropdown.Menu
            className="dropdown-menu-end shadow p-0"
            style={{
              maxHeight: "70vh",
              overflowY: "auto",
              minWidth: "280px",
            }}
          >
            {rootCategories.length === 0 ? (
              <Dropdown.Item disabled>Loading...</Dropdown.Item>
            ) : (
              <div
                className="d-none d-md-grid"
                style={{
                  gridTemplateColumns: "1fr 1fr",
                  display: "grid",
                  borderTop: "2px solid var(--bs-light)",
                  borderLeft: "2px solid var(--bs-light)",
                }}
              >
                {rootCategories.map((cat) => (
                  <Dropdown.Item
                    as={Link}
                    to={`/category/${cat.id}`}
                    key={cat.id}
                    className="d-flex align-items-center"
                    style={{
                      padding: "10px",
                      borderRight: "2px solid var(--bs-light)",
                      borderBottom: "2px solid var(--bs-light)",
                    }}
                  >
                    <img
                      src={`${baseUrl}${cat.imagePath}`}
                      alt={cat.name}
                      style={{
                        width: 60,
                        height: 60,
                        objectFit: "cover",
                        marginRight: 10,
                        borderRadius: 4,
                      }}
                    />
                    <span style={{ fontSize: "1rem", fontWeight: "400" }}>
                      {cat.name}
                    </span>
                  </Dropdown.Item>
                ))}
              </div>
            )}

            {/* Mobile layout */}
            {rootCategories.length > 0 && (
              <div className="d-block d-md-none">
                {rootCategories.map((cat) => (
                  <Dropdown.Item
                    as={Link}
                    to={`/category/${cat.id}`}
                    key={cat.id}
                    className="d-flex align-items-center border-bottom py-2 px-3"
                  >
                    <img
                      src={`${baseUrl}${cat.imagePath}`}
                      alt={cat.name}
                      style={{
                        width: 40,
                        height: 40,
                        objectFit: "cover",
                        marginRight: 10,
                        borderRadius: 4,
                      }}
                    />
                    <span style={{ fontSize: "0.95rem", fontWeight: "400" }}>
                      {cat.name}
                    </span>
                  </Dropdown.Item>
                ))}
              </div>
            )}
          </Dropdown.Menu>
        </Dropdown>

        <Form
          onSubmit={handleSearch}
          className="d-flex align-items-center flex-grow-1 mx-3"
          style={{ maxWidth: "800px" }}
        >
          <InputGroup>
            <InputGroup.Text className="bg-white">
              <FaSearch style={{ color: "bg-primary" }} />
            </InputGroup.Text>
            <Form.Control
              type="search"
              placeholder="Find everything you need"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
            <Button variant="warning" type="submit" className="text-black">
              Search
            </Button>
          </InputGroup>
        </Form>

        {!isAuthenticated ? (
          <>
            <Link to="/login" className="btn btn-outline-light btn-lg me-2">
              Login
            </Link>
            <Link to="/register" className="btn btn-outline-light btn-lg me-2">
              Register
            </Link>
          </>
        ) : (
          <Button
            variant="outline-light"
            size="lg"
            className="me-2"
            onClick={handleLogout}
          >
            Logout
          </Button>
        )}

        <Button
          as={Link as any}
          to="/cart"
          variant="outline-light"
          className="d-flex align-items-center"
        >
          <FaShoppingCart size={24} />
          {loading ? (
            <div
              className="ms-2 spinner-border text-warning"
              style={{ width: 16, height: 16 }}
            />
          ) : (
            <Badge bg="warning" pill className="ms-2 text-black">
              {cartCount ?? 0}
            </Badge>
          )}
        </Button>
      </div>
    </header>
  );
};

export default Header;
