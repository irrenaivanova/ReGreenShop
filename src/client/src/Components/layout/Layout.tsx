import { Outlet } from "react-router-dom";
import Footer from "./Footer";
import Header from "./Header";
import BottomHeader from "./BottomHeader";

const Layout = () => {
  return (
    <>
      <Header />
      <BottomHeader />
      <main className="container py-1 mb-5 mt-2">
        <Outlet />
      </main>
      <Footer />
    </>
  );
};

export default Layout;
