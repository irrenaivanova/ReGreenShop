import { createContext, useContext, useState, ReactNode } from "react";

interface ModalData {
  type: "success" | "error";
  message: string;
}

interface ModalContextType {
  modal: ModalData | null;
  showModal: (type: "success" | "error", message: string) => void;
  hideModal: () => void;
}

const ModalContext = createContext<ModalContextType | undefined>(undefined);

export const ModalProvider = ({ children }: { children: ReactNode }) => {
  const [modal, setModal] = useState<ModalData | null>(null);

  const showModal = (type: "success" | "error", message: string) => {
    setModal({ type, message });
  };

  const hideModal = () => {
    setModal(null);
  };

  return (
    <ModalContext.Provider value={{ modal, showModal, hideModal }}>
      {children}
    </ModalContext.Provider>
  );
};

export const useModal = () => {
  const context = useContext(ModalContext);
  if (!context) throw new Error("useModal must be used inside ModalProvider");
  return context;
};
