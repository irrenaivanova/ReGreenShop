import { Link } from "react-router-dom";

const NotFound = () => {
  return (
    <div
      className="card border-primary mx-auto my-5 p-4 text-center"
      style={{ maxWidth: "600px", borderRadius: "1.5rem" }}
    >
      <img
        src="/images/404.png"
        alt="Page Not Found"
        className="img-fluid mb-4"
      />
      <h2 className="mb-4">Sorry, the page you visited does not exist.</h2>
      <Link
        to="/"
        className="btn btn-primary mt-3"
        style={{ padding: "1rem 2.5rem", fontSize: "1.25rem" }}
      >
        Go to Homepage
      </Link>
    </div>
  );
};

export default NotFound;
