// src/MainRoutes.tsx
import { Routes, Route } from "react-router-dom";
import TopProducts from "../pages/TopProducts";
import Login from "../pages/Login";
import Register from "../pages/Register";
import Layout from "./Layout";

const MainRoutes = () => {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<TopProducts />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Route>
    </Routes>
  );
};

export default MainRoutes;
