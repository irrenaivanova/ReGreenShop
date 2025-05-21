import React, { useEffect, useState } from "react";
import { FaList, FaShoppingCart, FaSearch } from "react-icons/fa";
import { categoriesService } from "../services/categoriesService";
import { requestFactory } from "../lib/requester";

const request = requestFactory();

const Header = () => {
  const [rootCategories, setRootCategories] = useState([]);
  const [subCategories, setSubCategories] = useState([]);
  const [showSubcategoriesFor, setShowSubcategoriesFor] = useState(null);
  const [searchTerm, setSearchTerm] = useState("");
  const [cartCount, setCartCount] = useState(0);
  const [userLoggedIn, setUserLoggedIn] = useState(
    !!localStorage.getItem("jwt")
  );

  // Fetch root categories and cart count, and user logged-in status on mount
  useEffect(() => {
    categoriesService
      .getRootCategories()
      .then((res) => setRootCategories(res.data))
      .catch(() => setRootCategories([]));

    request
      .get("/api/cart")
      .then((res) => setCartCount(res.data.totalItems || 0))
      .catch(() => setCartCount(0));
  }, []);

  // Fetch subcategories when a root category is clicked
  const handleRootCategoryClick = (categoryId) => {
    if (showSubcategoriesFor === categoryId) {
      // Hide subcategories if clicked again
      setShowSubcategoriesFor(null);
      setSubCategories([]);
      return;
    }
    request
      .get(`/Category/GetSubCategoriesByRootCategory?categoryId=${categoryId}`)
      .then((res) => {
        setSubCategories(res.data);
        setShowSubcategoriesFor(categoryId);
      })
      .catch(() => {
        setSubCategories([]);
        setShowSubcategoriesFor(null);
      });
  };

  // Handle search submission
  const handleSearch = (e) => {
    e.preventDefault();
    if (!searchTerm.trim()) return;

    request
      .get(
        `/Product/ProductsBySearchString?searchString=${encodeURIComponent(
          searchTerm
        )}`
      )
      .then((res) => {
        console.log("Search results:", res.data);
        // Handle displaying search results or redirect
      })
      .catch((err) => {
        console.error("Search failed", err);
      });
  };

  // Logout
  const handleLogout = () => {
    localStorage.removeItem("jwt");
    setUserLoggedIn(false);
  };

  return (
    <header style={{ backgroundColor: "var(--bs-primary)", width: "100%" }}>
      <div className="container text-white py-3">
        {/* Logo and slogan */}
        <h1 className="mb-0">ReGreen Shop</h1>
        <small className="d-block mb-3">Shop Smart. Live Green.</small>

        {/* Buttons and Search */}
        <div className="d-flex align-items-center gap-3 flex-wrap">
          {/* Categories dropdown */}
          <div className="dropdown">
            <button
              className="btn btn-dark btn-lg dropdown-toggle d-flex align-items-center"
              type="button"
              id="categoriesDropdown"
              data-bs-toggle="dropdown"
              aria-expanded="false"
              // Prevent bootstrap toggling dropdown automatically because we control sub-menu visibility manually
              onClick={(e) => e.preventDefault()}
            >
              <FaList className="me-2" /> Categories
            </button>
            <ul
              className="dropdown-menu p-2"
              aria-labelledby="categoriesDropdown"
              style={{
                maxHeight: "400px",
                overflowY: "auto",
                minWidth: "300px",
              }}
            >
              {rootCategories.length === 0 && (
                <li className="dropdown-item disabled">Loading...</li>
              )}

              {rootCategories.map((cat) => (
                <li key={cat.id}>
                  <button
                    type="button"
                    className="dropdown-item d-flex justify-content-between align-items-center"
                    onClick={() => handleRootCategoryClick(cat.id)}
                    style={{
                      width: "100%",
                      textAlign: "left",
                      whiteSpace: "normal",
                    }}
                  >
                    {cat.name}
                    {/* Show indicator if subcategories shown */}
                    {showSubcategoriesFor === cat.id ? "▲" : "▼"}
                  </button>

                  {/* Show subcategories if this category is active */}
                  {showSubcategoriesFor === cat.id &&
                    subCategories.length > 0 && (
                      <ul className="list-unstyled ps-3 mt-1">
                        {subCategories.map((sub) => (
                          <li key={sub.id}>
                            <a
                              href={`/category/${sub.slug}`}
                              className="dropdown-item"
                              style={{ paddingLeft: "1rem" }}
                            >
                              {sub.name}
                            </a>
                          </li>
                        ))}
                      </ul>
                    )}
                </li>
              ))}
            </ul>
          </div>

          {/* Search form */}
          <form
            onSubmit={handleSearch}
            className="flex-grow-1 position-relative"
            style={{ maxWidth: "500px" }}
          >
            <input
              type="search"
              className="form-control form-control-lg ps-5"
              placeholder="Find everything you need"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              aria-label="Search"
            />
            <FaSearch
              style={{
                position: "absolute",
                top: "50%",
                left: "15px",
                transform: "translateY(-50%)",
                color: "yellow",
                pointerEvents: "none",
              }}
              size={20}
            />
          </form>

          {/* Auth buttons */}
          {!userLoggedIn ? (
            <>
              <a href="/login" className="btn btn-outline-light btn-lg">
                Login
              </a>
              <a href="/register" className="btn btn-outline-light btn-lg">
                Register
              </a>
            </>
          ) : (
            <>
              <button
                onClick={handleLogout}
                className="btn btn-outline-light btn-lg"
                type="button"
              >
                Logout
              </button>
            </>
          )}

          {/* Shopping cart */}
          <a href="/cart" className="position-relative text-white fs-4">
            <FaShoppingCart />
            {cartCount > 0 && (
              <span
                className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger"
                style={{ fontSize: "0.7rem" }}
              >
                {cartCount}
                <span className="visually-hidden">items in cart</span>
              </span>
            )}
          </a>
        </div>
      </div>
    </header>
  );
};

export default Header;
