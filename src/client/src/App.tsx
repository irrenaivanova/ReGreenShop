import { ErrorModal } from "./Components/common/ErrorModal";
import { SuccessModal } from "./Components/common/SuccessModal";
import MainRoutes from "./Components/layout/MainRoutes";
import { AuthProvider } from "./context/AuthContext";
import { ModalProvider, useModal } from "./context/ModalContext";

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
        <MainRoutes />
        <ModalRenderer />
      </ModalProvider>
    </AuthProvider>
  );
}

export default App;
