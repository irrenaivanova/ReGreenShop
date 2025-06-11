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
  FaIdBadge,
  FaSeedling,
  FaListUl,
} from "react-icons/fa";
import { Dropdown } from "react-bootstrap";
import { utilityService } from "../../services/utilityService";
import Logo from "../common/Logo";
import React from "react";
import { useAuth } from "../../context/AuthContext";

import NotificationDropdown from "../common/NotificationDropdown";
import { MdOutlineWorkHistory } from "react-icons/md";

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
  const { isAuthenticated } = useAuth();
  const { isAdmin } = useAuth();

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
            <div>
              <div className="fw-bold text-primary fs-6 mb-1">
                Shop Smart. Live Green.
              </div>
              <Link
                to="/regreen-rules"
                className="text-decoration-underline text-primary d-inline-flex align-items-center fs-6 fw-bold"
              >
                {/* <FaRecycle className="me-2 text-primary" /> */}
                ReGreenRules
              </Link>
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
        {isAuthenticated && (
          <div className="d-flex align-items-center">
            <div className="me-2">
              <NotificationDropdown />
            </div>
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

                {isAdmin && (
                  <>
                    <Dropdown.Divider />
                    <Dropdown.Header className="text-danger small px-3 text-decoration-underline">
                      Admin Area
                    </Dropdown.Header>
                    <Dropdown.Item
                      as={Link}
                      to="/pendingOrders"
                      className="fs-6 d-flex align-items-center"
                    >
                      <MdOutlineWorkHistory className="me-2 text-danger" />
                      Pending Orders
                    </Dropdown.Item>

                    <Dropdown.Item
                      as={Link}
                      to="/allProductsAdmin"
                      className="fs-6 d-flex align-items-center"
                    >
                      <FaListUl className="me-2 text-danger" />
                      All products
                    </Dropdown.Item>
                  </>
                )}
              </Dropdown.Menu>
            </Dropdown>
          </div>
        )}
      </div>
    </div>
  );
};

export default BottomHeader;
