const Spinner = () => {
  return (
    <div style={{ textAlign: "center", padding: "50px" }}>
      <div
        className="spinner"
        style={{
          border: "4px solid #f3f3f3",
          borderTop: "4px solid #157f83",
          borderRadius: "50%",
          width: "100px",
          height: "100px",
          animation: "spin 1s linear infinite",
          margin: "auto",
        }}
      />
      <style>
        {`@keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
          }`}
      </style>
    </div>
  );
};

export default Spinner;
