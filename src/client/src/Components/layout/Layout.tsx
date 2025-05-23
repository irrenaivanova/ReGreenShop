import { Outlet } from "react-router-dom";
import Footer from "./Footer";
import Header from "./Header";

const Layout = () => {
  return (
    <>
      <Header />
      <main className="container py-4">
        <Outlet />
      </main>
      <Footer />
    </>
  );
};

export default Layout;
