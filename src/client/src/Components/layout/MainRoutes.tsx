import { Routes, Route } from "react-router-dom";
import Login from "../pages/Login";
import Register from "../pages/Register";

import TopProducts from "../pages/TopProducts";
import Layout from "./Layout";
import SearchResults from "../pages/SearchResults";
import RootCategoryPage from "../pages/RootCategoryPage";
import ProductsBySubCategory from "../pages/ProductsBySubCategory";

const MainRoutes = () => {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<TopProducts />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/search" element={<SearchResults />} />
        <Route path="/category/:categoryId" element={<RootCategoryPage />} />
        <Route
          path="/subcategory/:categoryId"
          element={<ProductsBySubCategory />}
        />
      </Route>
    </Routes>
  );
};

export default MainRoutes;
