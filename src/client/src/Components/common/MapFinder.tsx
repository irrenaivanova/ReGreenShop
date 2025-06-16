import React, { useState } from "react";
import { GoogleMap, LoadScript, Marker } from "@react-google-maps/api";

const containerStyle = {
  width: "100%",
  height: "400px",
};

const defaultCenter = {
  lat: 42.6977, // Default: Sofia, Bulgaria
  lng: 23.3219,
};

const MapFinder: React.FC = () => {
  const [city, setCity] = useState("");
  const [address, setAddress] = useState("");
  const [number, setNumber] = useState("");
  const [location, setLocation] = useState<google.maps.LatLngLiteral | null>(
    null
  );

  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

  const handleFind = async () => {
    const fullAddress = `${address} ${number}, ${city}`;
    const geocoder = new window.google.maps.Geocoder();

    geocoder.geocode({ address: fullAddress }, (results, status) => {
      if (status === "OK" && results && results[0]) {
        const { lat, lng } = results[0].geometry.location;
        setLocation({ lat: lat(), lng: lng() });
      } else {
        alert("Address not found. Please check the input.");
      }
    });
  };

  if (!apiKey) {
    return <p>Missing Google Maps API key</p>;
  }

  return (
    <div>
      <div className="mb-3">
        <input
          type="text"
          placeholder="City"
          value={city}
          onChange={(e) => setCity(e.target.value)}
          className="form-control mb-2"
        />
        <input
          type="text"
          placeholder="Address"
          value={address}
          onChange={(e) => setAddress(e.target.value)}
          className="form-control mb-2"
        />
        <input
          type="text"
          placeholder="Number"
          value={number}
          onChange={(e) => setNumber(e.target.value)}
          className="form-control mb-2"
        />
        <button onClick={handleFind} className="btn btn-primary">
          Find Me
        </button>
      </div>

      <LoadScript googleMapsApiKey={apiKey}>
        <GoogleMap
          mapContainerStyle={containerStyle}
          center={location || defaultCenter}
          zoom={location ? 16 : 10}
        >
          {location && <Marker position={location} />}
        </GoogleMap>
      </LoadScript>
    </div>
  );
};

export default MapFinder;
