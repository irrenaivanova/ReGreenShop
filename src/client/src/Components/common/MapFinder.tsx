import React, { useState, useRef } from "react";
import {
  GoogleMap,
  Marker,
  Autocomplete,
  useJsApiLoader,
} from "@react-google-maps/api";

const containerStyle = {
  width: "100%",
  height: "400px",
};

const defaultCenter = {
  lat: 42.6977,
  lng: 23.3219,
};

const MapFinder: React.FC = () => {
  const [address, setAddress] = useState("");
  const [location, setLocation] = useState<google.maps.LatLngLiteral | null>(
    null
  );

  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY;

  const { isLoaded, loadError } = useJsApiLoader({
    googleMapsApiKey: apiKey || "",
    libraries: ["places"],
  });

  const autocompleteRef = useRef<google.maps.places.Autocomplete | null>(null);

  const onLoadAutocomplete = (
    autocomplete: google.maps.places.Autocomplete
  ) => {
    autocompleteRef.current = autocomplete;
  };

  const onPlaceChanged = () => {
    if (autocompleteRef.current !== null) {
      const place = autocompleteRef.current.getPlace();

      if (!place.geometry || !place.geometry.location) {
        alert("No details available for input: '" + place.name + "'");
        return;
      }

      const lat = place.geometry.location.lat();
      const lng = place.geometry.location.lng();

      setLocation({ lat, lng });
      setAddress(place.formatted_address || "");
    }
  };

  const handleMarkerDragEnd = (e: google.maps.MapMouseEvent) => {
    if (!e.latLng) return;

    const lat = e.latLng.lat();
    const lng = e.latLng.lng();

    setLocation({ lat, lng });

    // Reverse geocode to get address from lat,lng
    const geocoder = new window.google.maps.Geocoder();
    geocoder.geocode({ location: { lat, lng } }, (results, status) => {
      if (status === "OK" && results && results[0]) {
        setAddress(results[0].formatted_address || "");
      } else {
        console.warn("Reverse geocode failed: " + status);
      }
    });
  };

  if (loadError) return <p>Error loading Google Maps</p>;

  if (!isLoaded) return <p>Loading Map...</p>;

  return (
    <div>
      <div className="mb-3">
        <Autocomplete
          onLoad={onLoadAutocomplete}
          onPlaceChanged={onPlaceChanged}
        >
          <input
            type="text"
            placeholder="Enter address"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
            className="form-control mb-2"
          />
        </Autocomplete>
      </div>

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
  );
};

export default MapFinder;
