import { Routes, Route } from "react-router-dom";
import Login from "../pages/Login";
import Register from "../pages/Register";

import TopProducts from "../pages/TopProducts";
import Layout from "./Layout";
import SearchResults from "../pages/SearchResults";

const MainRoutes = () => {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<TopProducts />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/search" element={<SearchResults />} />
      </Route>
    </Routes>
  );
};

export default MainRoutes;
