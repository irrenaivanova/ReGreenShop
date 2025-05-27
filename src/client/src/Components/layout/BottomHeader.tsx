import { JSX, useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import {
  FaHeart,
  FaLeaf,
  FaTags,
  FaFire,
  FaClock,
  FaStar,
  FaGift,
  FaUser,
  FaBoxOpen,
  FaBell,
  FaIdBadge,
  FaSeedling,
} from "react-icons/fa";
import { Dropdown } from "react-bootstrap";
import { utilityService } from "../../services/utilityService";
import Logo from "../common/Logo";
import React from "react";

interface Label {
  id: number;
  name: string;
}

const labelIcons: { [key: string]: JSX.Element } = {
  "Top Offer": <FaTags className="me-2" />,
  Limited: <FaClock className="me-2" />,
  "New Arrival": <FaStar className="me-2" />,
  "Last Chance": <FaFire className="me-2" />,
  "Two for One": <FaGift className="me-2" />,
};

const BottomHeader = () => {
  const [labels, setLabels] = useState<Label[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    utilityService
      .getAllLabels()
      .then((res) => setLabels(res.data.data))
      .catch(() => setLabels([]));
  }, []);

  const handleLabelClick = (id: number) => {
    navigate(`/label/${id}`);
  };

  return (
    <div className="bg-light py-3 border-bottom">
      <div className="container d-flex flex-column flex-md-row justify-content-between align-items-center">
        <div className="text-center text-md-start mb-3 mb-md-0">
          <div className="d-flex align-items-center gap-2">
            <Logo size={50} />
            <div className="fw-bold text-primary fs-6">
              Shop Smart. Live Green.
            </div>
          </div>
        </div>

        <div className="d-flex flex-wrap justify-content-center">
          {labels.map((label) => (
            <button
              key={label.id}
              className="btn btn-outline-primary m-1 d-flex align-items-center"
              onClick={() => handleLabelClick(label.id)}
            >
              {(labelIcons[label.name] &&
                React.cloneElement(labelIcons[label.name], {
                  className: "me-2 text-warning",
                })) || <FaLeaf className="me-2 text-warning" />}{" "}
              {label.name}
            </button>
          ))}
        </div>

        <div className="d-flex align-items-center">
          <Link to="/favorites" className="btn btn-outline-primary me-2">
            <FaHeart size={20} />
          </Link>
          <Dropdown>
            <Dropdown.Toggle variant="outline-primary" id="userDropdown">
              <FaUser size={20} />
            </Dropdown.Toggle>

            <Dropdown.Menu className="shadow">
              <Dropdown.Item
                as={Link}
                to="/my-orders"
                className="fs-6 d-flex align-items-center"
              >
                <FaBoxOpen className="me-2 text-warning" />
                My Orders
              </Dropdown.Item>
              <Dropdown.Item
                as={Link}
                to="/my-notifications"
                className="fs-6 d-flex align-items-center"
              >
                <FaBell className="me-2 text-warning" />
                My Notifications
              </Dropdown.Item>
              <Dropdown.Item
                as={Link}
                to="/my-info"
                className="fs-6 d-flex align-items-center"
              >
                <FaIdBadge className="me-2 text-warning" />
                My Info
              </Dropdown.Item>
              <Dropdown.Item
                as={Link}
                to="/my-impact"
                className="fs-6 d-flex align-items-center"
              >
                <FaSeedling className="me-2 text-warning" />
                My ReGreen Impact
              </Dropdown.Item>
            </Dropdown.Menu>
          </Dropdown>
        </div>
      </div>
    </div>
  );
};

export default BottomHeader;
