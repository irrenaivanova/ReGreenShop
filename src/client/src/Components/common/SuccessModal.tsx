import { useEffect } from "react";
export const SuccessModal = ({
  message,
  onClose,
}: {
  message: string;
  onClose: () => void;
}) => {
  useEffect(() => {
    const timer = setTimeout(onClose, 3000);
    return () => clearTimeout(timer);
  }, [onClose]);

  return (
    <div
      className="position-fixed p-3"
      style={{ zIndex: 1055, top: "150px", right: "40px" }}
    >
      <div
        className="toast show text-white bg-primary border-0 shadow-sm"
        style={{ minWidth: "300px" }}
      >
        <div
          className="toast-body"
          style={{ fontSize: "1.1rem", padding: "1rem" }}
        >
          {message}
        </div>
      </div>
    </div>
  );
};
