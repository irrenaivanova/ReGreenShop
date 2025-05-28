import { useEffect, useState } from "react";
import { Voucher } from "../../types/Voucher";
import { utilityService } from "../../services/utilityService";

interface Props {
  availableGreenPoints: number;
  onSelectVoucher: (voucherId: number | null) => void;
}

const VoucherSelector = ({ availableGreenPoints, onSelectVoucher }: Props) => {
  const [vouchers, setVouchers] = useState<Voucher[]>([]);
  const [selectedId, setSelectedId] = useState<number | null>(null);

  useEffect(() => {
    utilityService
      .getAllVouchers()
      .then((res) => setVouchers(res.data.data))
      .catch((err) => console.error("Failed to fetch vouchers", err));
  }, []);

  const handleChange = (id: number) => {
    setSelectedId(id);
    onSelectVoucher(id);
  };

  const anyAvailable = vouchers.some(
    (v) => v.greenPoints <= availableGreenPoints
  );

  return (
    <div className="card p-4 border-primary mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h5 className="mb-0">
          You have <strong>{availableGreenPoints}</strong> green points
        </h5>
        <small
          className={`ms-3  fw-bold ${
            anyAvailable ? "text-primary fs-5" : "text-danger fs-5"
          }`}
        >
          {anyAvailable
            ? "You can use a voucher. Please choose one below"
            : `You need more points to use a voucher`}
        </small>
      </div>

      <div className="d-flex flex-wrap gap-3">
        {vouchers.map((voucher) => {
          const disabled = voucher.greenPoints > availableGreenPoints;
          return (
            <label
              key={voucher.id}
              className={`border rounded p-3 flex-fill ${
                disabled ? "bg-light text-muted" : "bg-white"
              }`}
              style={{
                minWidth: "250px",
                cursor: disabled ? "not-allowed" : "pointer",
              }}
            >
              <input
                type="radio"
                className="form-check-input me-2"
                name="voucher"
                value={voucher.id}
                disabled={disabled}
                checked={selectedId === voucher.id}
                onChange={() => handleChange(voucher.id)}
              />
              <div>
                <div>
                  <strong>Discount:</strong> {voucher.priceDiscount.toFixed(2)}{" "}
                  lv
                </div>
                <div>
                  <strong>Green Points Required:</strong> {voucher.greenPoints}
                </div>
              </div>
            </label>
          );
        })}
      </div>
    </div>
  );
};

export default VoucherSelector;
