import { Routes, Route } from "react-router-dom";
import Login from "../pages/Login";
import Register from "../pages/Register";

import TopProducts from "../pages/TopProducts";
import Layout from "./Layout";
import SearchResults from "../pages/SearchResults";
import RootCategoryPage from "../pages/RootCategoryPage";
import ProductsBySubCategory from "../pages/ProductsBySubCategory";
import ProductsByLabel from "../pages/ProductsByLabel";
import MyFavorites from "../pages/MyFavorites";
import ReGreenMission from "../pages/ReGreenMission";
import NotFound from "../pages/NotFound";
import ProductDetails from "../pages/ProductDetails";
import Cart from "../pages/Cart";

import MyOrders from "../pages/MyOrders";
import PendingOrders from "../pages/PendingOrders";
import GreenImpact from "../pages/GreenImpact";
import MyInfo from "../pages/MyInfo";
import GoogleLoginRedirect from "../pages/GoogleLoginRedirect";
import AllProductsPaginated from "../pages/AllProductsPaginated";
import Checkout from "../pages/Checkout";
import StripeSuccess from "../pages/StripeSuccess";
import StripeFail from "../pages/StripeFail";

const MainRoutes = () => {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route path="/" element={<TopProducts />} />
        <Route path="/favorites" element={<MyFavorites />} />
        <Route path="/regreen-rules" element={<ReGreenMission />} />
        <Route path="/login" element={<Login />} />
        <Route path="/google-login" element={<GoogleLoginRedirect />} />
        <Route path="/register" element={<Register />} />
        <Route path="/stripe-success" element={<StripeSuccess />} />
        <Route path="/stripe-fail" element={<StripeFail />} />
        <Route path="/cart" element={<Cart />} />
        <Route path="/my-orders" element={<MyOrders />} />
        <Route path="/my-info" element={<MyInfo />} />
        <Route path="/pendingOrders" element={<PendingOrders />} />
        <Route path="/checkout" element={<Checkout />} />
        <Route path="/search" element={<SearchResults />} />
        <Route path="/my-impact" element={<GreenImpact />} />
        <Route path="/allProductsAdmin" element={<AllProductsPaginated />} />
        <Route path="/category/:categoryId" element={<RootCategoryPage />} />
        <Route path="/label/:labelId" element={<ProductsByLabel />} />
        <Route path="/product/:productId" element={<ProductDetails />} />
        <Route
          path="/subcategory/:categoryId"
          element={<ProductsBySubCategory />}
        />
        <Route path="/*" element={<NotFound />} />
      </Route>
    </Routes>
  );
};

export default MainRoutes;
