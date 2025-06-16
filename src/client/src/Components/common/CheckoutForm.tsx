import { useState, useEffect, useRef } from "react";
import { useForm, Controller } from "react-hook-form";
import { addHours, addDays } from "date-fns";
import DOMPurify from "dompurify";
import DatePicker from "react-datepicker";
import {
  GoogleMap,
  Marker,
  Autocomplete,
  useJsApiLoader,
} from "@react-google-maps/api";
import { utilityService } from "../../services/utilityService";
import "react-datepicker/dist/react-datepicker.css";

interface Props {
  userInfo: {
    firstName: string;
    lastName: string;
    address: string;
    totalGreenPoints: number;
  };
  onFormSubmit: (data: any, paymentMethodId: number) => void;
}

interface PaymentMethod {
  id: number;
  name: string;
}

const containerStyle = {
  width: "100%",
  height: "300px",
};

const defaultCenter = {
  lat: 42.6977,
  lng: 23.3219,
};

const CheckoutForm = ({ userInfo, onFormSubmit }: Props) => {
  const {
    register,
    handleSubmit,
    control,
    setValue,
    setError,
    formState: { errors },
  } = useForm();

  const [paymentMethods, setPaymentMethods] = useState<PaymentMethod[]>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Google Maps API Loader with Places library
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY || "";
  const { isLoaded, loadError } = useJsApiLoader({
    googleMapsApiKey: apiKey,
    libraries: ["places"],
  });
  const isAddressInSofia = (address: string) => {
    return address.toLowerCase().includes("sofia");
  };
  const autocompleteRef = useRef<google.maps.places.Autocomplete | null>(null);
  const [location, setLocation] = useState<google.maps.LatLngLiteral | null>(
    null
  );
  const [fullAddress, setFullAddress] = useState("");

  // Valid delivery time check
  const isValidDeliveryTime = (time: Date) => {
    const now = new Date();
    const fourHoursFromNow = addHours(now, 4);
    const hour = time.getHours();
    return time > fourHoursFromNow && hour >= 9 && hour <= 20;
  };

  const filterTimes = (time: Date) => isValidDeliveryTime(time);

  // Load payment methods on mount
  useEffect(() => {
    utilityService
      .getAllPaymentMethods()
      .then((res) => setPaymentMethods(res.data.data));
  }, []);

  // Initialize form & address/map from userInfo.address
  useEffect(() => {
    if (!userInfo) return;

    setValue("firstName", userInfo.firstName);
    setValue("lastName", userInfo.lastName);
    setValue("paymentMethodId", ""); // Reset payment method on load

    setFullAddress(userInfo.address);
    setValue("fullAddress", userInfo.address);

    // Geocode to get lat/lng from address string
    if (isLoaded && userInfo.address) {
      const geocoder = new window.google.maps.Geocoder();
      geocoder.geocode({ address: userInfo.address }, (results, status) => {
        if (status === "OK" && results && results[0]) {
          const loc = results[0].geometry.location;
          setLocation({ lat: loc.lat(), lng: loc.lng() });
        } else {
          setLocation(null);
        }
      });
    }
  }, [userInfo, setValue, isLoaded]);

  // Autocomplete load
  const onLoadAutocomplete = (
    autocomplete: google.maps.places.Autocomplete
  ) => {
    autocompleteRef.current = autocomplete;
  };

  // When place selected from autocomplete
  const onPlaceChanged = () => {
    if (autocompleteRef.current !== null) {
      const place = autocompleteRef.current.getPlace();
      if (!place.geometry || !place.geometry.location) {
        alert(
          "ReGreenShop: No details available for input: '" + place.name + "'"
        );
        return;
      }
      const lat = place.geometry.location.lat();
      const lng = place.geometry.location.lng();
      const formattedAddress = place.formatted_address || "";

      if (!isAddressInSofia(formattedAddress)) {
        alert("ReGreenShop: Please enter an address in Sofia.");
        return;
      }
      setLocation({ lat, lng });
      setFullAddress(place.formatted_address || "");
      setValue("fullAddress", place.formatted_address || "");
    }
  };

  // When marker is dragged, update position and address input via reverse geocoding
  const handleMarkerDragEnd = (e: google.maps.MapMouseEvent) => {
    if (!e.latLng) return;
    const lat = e.latLng.lat();
    const lng = e.latLng.lng();
    setLocation({ lat, lng });

    const geocoder = new window.google.maps.Geocoder();
    geocoder.geocode({ location: { lat, lng } }, (results, status) => {
      if (status === "OK" && results && results[0]) {
        const addr = results[0].formatted_address || "";

        if (!isAddressInSofia(addr)) {
          alert("ReGreenShop: Please enter an address in Sofia.");
          return;
        }
        setFullAddress(addr || "");
        setValue("fullAddress", addr || "");
      }
    });
  };

  // On submit, sanitize & send
  const handleInternalSubmit = async (data: any) => {
    setIsSubmitting(true);

    if (
      !data.deliveryDateTime ||
      !isValidDeliveryTime(new Date(data.deliveryDateTime))
    ) {
      setError("deliveryDateTime", {
        type: "manual",
        message:
          "Delivery time must be at least 4 hours from now and between 9:00–20:00.",
      });
      setIsSubmitting(false);
      return;
    }

    const sanitizedData = {
      ...data,
      firstName: DOMPurify.sanitize(data.firstName),
      lastName: DOMPurify.sanitize(data.lastName),
      fullAddress: DOMPurify.sanitize(data.fullAddress),
    };

    await onFormSubmit(sanitizedData, sanitizedData.paymentMethodId);
    setIsSubmitting(false);
  };

  if (loadError) return <p>Error loading Google Maps</p>;
  if (!isLoaded) return <p>Loading Map...</p>;

  const minDate = new Date();
  const maxDate = addDays(minDate, 5);

  const renderError = (error: any) =>
    error?.message ? (
      <small className="text-danger">{String(error.message)}</small>
    ) : null;

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
          {renderError(errors.firstName)}
        </div>
        <div className="col-md-6">
          <label className="form-label">Last Name</label>
          <input
            className="form-control"
            {...register("lastName", { required: "Last name is required." })}
          />
          {renderError(errors.lastName)}
        </div>

        <div className="col-md-12">
          <label className="form-label">Address (Full)</label>
          <Autocomplete
            onLoad={onLoadAutocomplete}
            onPlaceChanged={onPlaceChanged}
          >
            <input
              type="text"
              className="form-control"
              placeholder="Enter your address"
              {...register("fullAddress", { required: "Address is required." })}
              value={fullAddress}
              onChange={(e) => {
                setFullAddress(e.target.value);
                setValue("fullAddress", e.target.value);
              }}
            />
          </Autocomplete>
          {renderError(errors.fullAddress)}
        </div>

        {/* Map */}
        <div className="col-md-12">
          <GoogleMap
            mapContainerStyle={containerStyle}
            center={location || defaultCenter}
            zoom={location ? 16 : 10}
          >
            {location && (
              <Marker
                position={location}
                draggable={true}
                onDragEnd={handleMarkerDragEnd}
              />
            )}
          </GoogleMap>
        </div>

        {/* Payment Method */}
        <div className="col-md-6">
          <label className="form-label">Payment Method</label>
          <select
            className="form-select"
            {...register("paymentMethodId", {
              required: "Payment method is required.",
            })}
          >
            <option value="">Select payment method</option>
            {paymentMethods.map((pm) => (
              <option key={pm.id} value={pm.id}>
                {pm.name}
              </option>
            ))}
          </select>
          {renderError(errors.paymentMethodId)}
        </div>

        {/* Delivery Time */}
        <div className="col-md-6">
          <label className="form-label mb-2">Delivery Time</label>
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
          {renderError(errors.deliveryDateTime)}
        </div>

        <div className="col-md-12 d-flex justify-content-end mt-3">
          <button
            type="submit"
            className="btn btn-primary btn-lg"
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
