import { useEffect, useState } from "react";
import { Card, Container, Spinner, Row, Col } from "react-bootstrap";
import { userService } from "../../services/userService";
import { FaMapMarkerAlt, FaUser } from "react-icons/fa";

interface Address {
  street: string;
  number: number;
  cityName: string;
}

interface UserInfo {
  firstName: string;
  lastName: string;
  totalGreenPoints: number;
  addresses: Address[];
}

const MyInfo = () => {
  const [userInfo, setUserInfo] = useState<UserInfo | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    userService
      .getUserInfo()
      .then((res) => {
        setUserInfo(res.data.data);
      })
      .finally(() => setLoading(false));
  }, []);

  if (loading)
    return <Spinner animation="border" className="d-block mx-auto my-5" />;

  if (!userInfo)
    return <p className="text-danger text-center">Failed to load user info.</p>;

  return (
    <Container className="my-5 col-md-8">
      <Card className="border-primary rounded-4 shadow p-4">
        <Card.Body>
          <Card.Title className="text-primary fs-4 d-flex align-items-center mb-4">
            <FaUser className="me-2" />
            {userInfo.firstName} {userInfo.lastName}
          </Card.Title>

          <p className="fs-5">
            <strong>Total Green Points:</strong>{" "}
            <span className="text-success fw-bold">
              {userInfo.totalGreenPoints}
            </span>
          </p>

          <hr className="my-4 border-primary border-2" />

          <h5 className="text-secondary mb-3">Addresses:</h5>
          <Row>
            {userInfo.addresses.map((address, idx) => (
              <Col md={6} key={idx}>
                <div className="mb-3 d-flex align-items-start">
                  <FaMapMarkerAlt className="text-warning me-2 mt-1" />
                  <div>
                    {address.street} №{address.number}, {address.cityName}
                  </div>
                </div>
              </Col>
            ))}
          </Row>
        </Card.Body>
      </Card>
    </Container>
  );
};

export default MyInfo;
