// src/MainRoutes.tsx
import { Routes, Route } from "react-router-dom";
import TopProducts from "./TopProducts";
import Login from "./Login";
import Register from "./Register";
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
