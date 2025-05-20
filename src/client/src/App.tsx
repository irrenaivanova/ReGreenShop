import "./App.css";
import { Routes, Route } from "react-router-dom";
import TopProducts from "./Components/TopProducts";
import { ModalProvider, useModal } from "./context/ModalContext";
import { AuthProvider } from "./context/AuthContext";
import Login from "./Components/Login";
import { SuccessModal } from "./Components/SuccessModal";
import { ErrorModal } from "./Components/ErrorModal";

function ModalRenderer() {
  const { modal, hideModal } = useModal();

  if (!modal) return null;

  return modal.type === "success" ? (
    <SuccessModal message={modal.message} onClose={hideModal} />
  ) : (
    <ErrorModal message={modal.message} onClose={hideModal} />
  );
}

function App() {
  return (
    <AuthProvider>
      <ModalProvider>
        <ModalRenderer />

        <Routes>
          <Route path="/" element={<TopProducts />} />
          <Route path="/login" element={<Login />} />
          {/* <Route path="/products/:id" element={<ProductDetails />} /> */}
        </Routes>
      </ModalProvider>
    </AuthProvider>
  );
}

export default App;
