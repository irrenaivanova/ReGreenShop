import { useModal } from "../../context/ModalContext";
import { ErrorModal } from "./ErrorModal";
import { SuccessModal } from "./SuccessModal";

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
