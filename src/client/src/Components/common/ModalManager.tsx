import { SuccessModal } from "./SuccessModal";
import { ErrorModal } from "./common/ErrorModal";
import { useModal } from "../context/ModalContext";

const ModalManager = () => {
  const { modal, hideModal } = useModal();

  if (!modal) return null;

  return modal.type === "success" ? (
    <SuccessModal message={modal.message} onClose={hideModal} />
  ) : (
    <ErrorModal message={modal.message} onClose={hideModal} />
  );
};

export default ModalManager;
