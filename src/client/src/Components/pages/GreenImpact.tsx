import { useEffect, useState } from "react";
import { Card, Col, Container, Row } from "react-bootstrap";
import { userService } from "../../services/userService";
import Spinner from "../common/Spinner";
import { FaBoxOpen, FaGlobeAmericas, FaLeaf, FaRecycle } from "react-icons/fa";

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
    <Container className="my-4 col-md-10">
      <Card className="border-primary rounded-4 shadow-sm p-4">
        <Card.Body>
          <h2 className="text-primary mb-4 text-center">Total Green Impact</h2>
          <hr className="my-5 border-warning border-4" />

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
                          ? "Total " +
                            item.name[2].toUpperCase() +
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

          <hr className="my-5 border-warning border-4" />
          <div className="text-center px-4">
            <h3 className="text-primary mb-3">
              <FaRecycle className="me-2" />
              Why It Matters
            </h3>
            <p className="text-muted fs-5">
              <FaGlobeAmericas className="me-2 text-primary" />
              Every item returned makes a difference — together, you've helped
              recycle bottles, cans, and bags through our delivery system.
            </p>
            <p className="text-muted fs-6">
              <FaBoxOpen className="me-2 text-primary" />
              But recycling is just one step. Each day, we use plastic and
              packaging that may never be recovered.
            </p>
            <p className="text-muted fs-6">
              <FaLeaf className="me-2 text-primary" />
              Let this impact inspire bigger action — to reduce, reuse, and
              protect our planet. 🌱
            </p>
          </div>
        </Card.Body>
      </Card>
    </Container>
  );
};

export default GreenImpact;
