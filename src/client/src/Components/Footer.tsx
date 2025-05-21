const Footer = () => (
  <footer className="bg-primary text-light text-center py-4">
    <div className="container d-flex justify-content-center align-items-center flex-wrap gap-2 small">
      <span>
        &copy; {new Date().getFullYear()}{" "}
        <strong>ReGreenShop – Shop Smart. Live Green.</strong>
      </span>
      <a
        href="https://github.com/irrenaivanova/ReGreenShop"
        target="_blank"
        rel="noopener noreferrer"
        className="text-light text-decoration-underline ms-5"
      >
        View source code on GitHub
      </a>
    </div>
  </footer>
);
export default Footer;
