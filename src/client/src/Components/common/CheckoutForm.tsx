import { useForm, Controller } from "react-hook-form";
import { useEffect, useState } from "react";
import { addHours, addDays } from "date-fns";
import DOMPurify from "dompurify";
import DatePicker from "react-datepicker";
import { utilityService } from "../../services/utilityService";
import "react-datepicker/dist/react-datepicker.css";

interface Props {
  userInfo: {
    firstName: string;
    lastName: string;
    street: string;
    number: number;
    cityId: number;
    cityName: string;
    totalGreenPoints: number;
  };
  onFormSubmit: (data: any) => void;
}

interface City {
  id: number;
  name: string;
}

interface PaymentMethod {
  id: number;
  name: string;
}

const CheckoutForm = ({ userInfo, onFormSubmit }: Props) => {
  const {
    register,
    handleSubmit,
    control,
    setValue,
    formState: { errors },
  } = useForm();

  const [cities, setCities] = useState<City[]>([]);
  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleInternalSubmit = async (data: any) => {
    setIsSubmitting(true);
    await onFormSubmit(data);
    setIsSubmitting(false);
  };
  useEffect(() => {
    utilityService.getAllCities().then((res) => setCities(res.data.data));
    utilityService
      .getAllPaymentMethods()
      .then((res) => setPaymentMethods(res.data.data));
  }, []);

  useEffect(() => {
    if (!userInfo) return;
    setValue("firstName", userInfo.firstName);
    setValue("lastName", userInfo.lastName);
    setValue("street", userInfo.street);
    setValue("number", userInfo.number);
    setValue("cityId", userInfo.cityId);
  }, [userInfo, setValue]);

  const filterTimes = (time: Date) => {
    const now = new Date();
    const fourHoursFromNow = addHours(now, 4);
    const hour = time.getHours();
    return time > fourHoursFromNow && hour >= 9 && hour <= 20;
  };

  const minDate = new Date();
  const maxDate = addDays(minDate, 5);

  return (
    <div className="card p-4 border-primary mt-4 col-md-10 offset-md-1">
      <h4 className="mb-3">Delivery Information</h4>
      <form onSubmit={handleSubmit(handleInternalSubmit)} className="row g-3">
        <div className="col-md-6">
          <label className="form-label">First Name</label>
          <input
            className="form-control"
            {...register("firstName", { required: "First name is required." })}
          />
          {errors.firstName?.message && (
            <small className="text-danger">{`${errors.firstName.message}`}</small>
          )}
        </div>

        <div className="col-md-6">
          <label className="form-label">Last Name</label>
          <input
            className="form-control"
            {...register("lastName", { required: "Last name is required." })}
          />
          {errors.lastName?.message && (
            <small className="text-danger">{`${errors.lastName.message}`}</small>
          )}
        </div>

        <div className="col-md-8">
          <label className="form-label">Street</label>
          <input
            className="form-control"
            {...register("street", { required: "Street is required." })}
          />
          {errors.street?.message && (
            <small className="text-danger">{`${errors.street.message}`}</small>
          )}
        </div>

        <div className="col-md-4">
          <label className="form-label">Number</label>
          <input
            type="number"
            className="form-control"
            {...register("number", {
              required: "Number is required.",
              min: 1,
            })}
          />
          {errors.number?.message && (
            <small className="text-danger">{`${errors.number.message}`}</small>
          )}
        </div>
        <div className="col-md-6">
          <label className="form-label">City</label>
          <select
            className="form-select"
            {...register("cityId", { required: true })}
            defaultValue={userInfo.cityId?.toString() || ""}
          >
            <option value="" disabled>
              Select city
            </option>
            {cities.map((c) => (
              <option key={c.id} value={c.id}>
                {c.name}
              </option>
            ))}
          </select>
          {errors.cityId && (
            <small className="text-danger">City is required.</small>
          )}
        </div>

        <div className="col-md-6">
          <label className="form-label">Payment Method</label>
          <select
            className="form-select"
            {...register("paymentMethodId", { required: true })}
          >
            <option value="">Select payment method</option>
            {paymentMethods.map((pm) => (
              <option key={pm.id} value={pm.id}>
                {pm.name}
              </option>
            ))}
          </select>
          {errors.paymentMethodId && (
            <small className="text-danger">Payment method is required.</small>
          )}
        </div>
        <div className="col-md-9">
          <label className="form-label mb-2">Delivery Time</label>
          <div>
            <Controller
              control={control}
              name="deliveryDateTime"
              rules={{ required: "Delivery time is required." }}
              render={({ field }) => (
                <DatePicker
                  {...field}
                  selected={field.value}
                  showTimeSelect
                  filterTime={filterTimes}
                  minDate={minDate}
                  maxDate={maxDate}
                  dateFormat="MMMM d, yyyy h:mm aa"
                  placeholderText="Select a delivery time"
                  className="form-control"
                  timeIntervals={60}
                  popperPlacement="bottom-start"
                />
              )}
            />
          </div>
          {errors.deliveryDateTime?.message && (
            <small className="text-danger">{`${errors.deliveryDateTime.message}`}</small>
          )}
        </div>
        <div className="col-md-3 d-flex align-items-end justify-content-end">
          <button
            type="submit"
            className="btn btn-primary btn-lg mt-3"
            disabled={isSubmitting}
          >
            {isSubmitting ? "Placing your order..." : "Confirm Order"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CheckoutForm;
