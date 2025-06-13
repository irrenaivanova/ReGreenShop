import { Outlet } from "react-router-dom";
import Footer from "./Footer";
import Header from "./Header";
import BottomHeader from "./BottomHeader";
import ChatWidget from "../common/ChatWidget";

const Layout = () => {
  return (
    <>
      <Header />
      <BottomHeader />
      <main className="container py-1 mb-5 mt-2">
        <Outlet />
      </main>
      <Footer />
      <ChatWidget />
    </>
  );
};

export default Layout;
