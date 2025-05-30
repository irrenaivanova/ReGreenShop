import { useEffect, useState } from "react";
import { Dropdown, Badge, Spinner } from "react-bootstrap";
import {
  MdOutlineNotifications,
  MdDone,
  MdNotificationsActive,
} from "react-icons/md";
import { userService } from "../../services/userService";

interface Notification {
  title: string;
  text: string;
}

const NotificationDropdown = () => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [showDropdown, setShowDropdown] = useState(false);
  const [lastUpdated, setLastUpdated] = useState<Date | null>(null);

  const fetchNotifications = async () => {
    try {
      const res = await userService.getAllUnReadNotifications();
      setNotifications(res.data.data || []);
      setLastUpdated(new Date());
    } catch (err) {
      console.error("Error fetching notifications", err);
    }
  };

  const handleMarkAsRead = async () => {
    try {
      await userService.readNotifications();
      setNotifications([]);
    } catch (err) {
      console.error("Error marking notifications as read", err);
    }
  };

  const toggleDropdown = async (nextShow: boolean) => {
    setShowDropdown(nextShow);
    if (nextShow) {
      setIsLoading(true);
      await fetchNotifications();
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchNotifications();
    const interval = setInterval(fetchNotifications, 30000); // every 30 seconds
    return () => clearInterval(interval);
  }, []);

  return (
    <Dropdown show={showDropdown} onToggle={toggleDropdown}>
      <Dropdown.Toggle
        as="div"
        role="button"
        className="position-relative d-inline-block"
        style={{
          cursor: "pointer",
          padding: 0,
        }}
      >
        <MdOutlineNotifications size={28} className="text-primary" />

        {notifications.length > 0 && (
          <Badge
            bg="warning"
            pill
            className="position-absolute top-0 start-50 translate-middle text-black"
            style={{
              fontSize: "0.65rem",
              minWidth: "18px",
              height: "18px",
              padding: "0 6px",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              borderRadius: "10px",
              zIndex: 1,
            }}
          >
            {notifications.length}
          </Badge>
        )}
      </Dropdown.Toggle>

      <Dropdown.Menu style={{ width: "320px" }} align="end">
        <Dropdown.Header className="d-flex justify-content-between align-items-center">
          Notifications
          {lastUpdated && (
            <small className="text-muted ms-2" style={{ fontSize: "0.7rem" }}>
              Update: {lastUpdated.toLocaleString()}
            </small>
          )}
        </Dropdown.Header>

        {isLoading ? (
          <div className="text-center p-3">
            <Spinner animation="border" size="sm" />
          </div>
        ) : notifications.length === 0 ? (
          <div className="text-center p-3 text-muted">No new notifications</div>
        ) : (
          notifications.map((n, index) => (
            <div key={index}>
              <Dropdown.Item
                className="d-flex gap-3 align-items-start py-3"
                style={{ whiteSpace: "normal" }}
              >
                <MdNotificationsActive
                  size={32}
                  className="text-warning flex-shrink-0 mt-1"
                />

                <div>
                  <strong style={{ textTransform: "none" }}>{n.title}</strong>
                  <div style={{ fontSize: "0.9rem", textTransform: "none" }}>
                    {n.text}
                  </div>
                </div>
              </Dropdown.Item>
              {index < notifications.length - 1 && (
                <div className="border-top mx-3" />
              )}
            </div>
          ))
        )}
        <div className="text-end px-3 pb-2">
          <MdDone
            size={24}
            className="text-success cursor-pointer"
            onClick={handleMarkAsRead}
            title="Mark all as read"
          />
        </div>
      </Dropdown.Menu>
    </Dropdown>
  );
};

export default NotificationDropdown;
