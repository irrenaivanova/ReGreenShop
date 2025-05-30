import { useEffect, useState } from "react";
import { Card, Col, Container, Row } from "react-bootstrap";
import { userService } from "../../services/userService";
import Spinner from "../common/Spinner";

interface ImpactItem {
  name: string;
  quantity: number;
}

const GreenImpact = () => {
  const [impactData, setImpactData] = useState<ImpactItem[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    userService
      .getTotalGreenImpact()
      .then((res) => {
        setImpactData(res.data.data || []);
      })
      .finally(() => {
        setLoading(false);
      });
  }, []);

  if (loading)
    return (
      <div className="text-center">
        <Spinner />
      </div>
    );

  return (
    <Container className="my-4">
      <h4 className="text-primary mb-4 text-center">Total Green Impact</h4>
      <Row className="g-3">
        {impactData.map((item, idx) => {
          const isLastTwo =
            impactData.length % 3 !== 0 &&
            idx >= impactData.length - (impactData.length % 3);
          return (
            <Col
              md={4}
              key={idx}
              className={
                isLastTwo && impactData.length % 3 === 2 ? "mx-auto" : ""
              }
            >
              <Card className="border-primary rounded-4 shadow-sm h-100">
                <Card.Body>
                  <Card.Title className="text-primary">
                    {item.name.length > 2
                      ? item.name[2].toUpperCase() +
                        item.name.slice(3, 21) +
                        "s"
                      : item.name}
                  </Card.Title>
                  <Card.Text>
                    <span className="fs-5 fw-medium">Total:</span>{" "}
                    <span className="fs-3 fw-bold text-primary">
                      {item.quantity}
                    </span>
                  </Card.Text>
                </Card.Body>
              </Card>
            </Col>
          );
        })}
      </Row>
    </Container>
  );
};

export default GreenImpact;
