import { useEffect, useState } from "react";
import { Order } from "../../types/Order";
import { orderService } from "../../services/orderService";
import { useModal } from "../../context/ModalContext";
import { Col, Container, Row, Table } from "react-bootstrap";
import Spinner from "../common/Spinner";
import { baseUrl } from "../../Constants/baseUrl";
import GreenForm from "../common/GreenForm";

const PendingOrders = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const { showModal } = useModal();
  const [visibleFormOrderId, setVisibleFormOrderId] = useState<string | null>(
    null
  );

  useEffect(() => {
    const fetchOrders = async () => {
      try {
        const res = await orderService.getPendingOrders();
        setOrders(res.data.data || []);
      } catch (err: any) {
        const errorMessage = err.response?.data?.error || "Login failed.";
        showModal?.("error", errorMessage);
      } finally {
        setIsLoading(false);
      }
    };
    fetchOrders();
  }, []);

  const toggleForm = (orderId: string) => {
    if (visibleFormOrderId === orderId) {
      setVisibleFormOrderId(null);
    } else {
      setVisibleFormOrderId(orderId);
    }
  };

  return (
    <Container className="my-4">
      <Row className="justify-content-center">
        <Col md={10}>
          <h4 className="mb-4 text-primary">My Orders</h4>

          {isLoading ? (
            <div className="text-center py-5">
              <Spinner />
            </div>
          ) : orders.length === 0 ? (
            <div className="text-center text-muted fs-5">
              You have no orders yet.
            </div>
          ) : (
            <Table striped bordered hover responsive>
              <thead className="table-primary">
                <tr>
                  <th>#</th>
                  <th>Id</th>
                  <th>Date</th>
                  <th>Status</th>
                  <th>Address</th>
                  <th>Payment</th>
                  <th>TotalPrice</th>
                  <th>Invoice</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order, index) => (
                  <tr key={order.id}>
                    <td>{index + 1}</td>
                    <td>{order.id.slice(0, 6)}...</td>
                    <td>{order.createdOn}</td>
                    <td>{order.status}</td>
                    <td>{order.address}</td>
                    <td>{order.payment}</td>
                    <td>{order.totalPrice} lv</td>
                    <td>
                      <a
                        href={`${baseUrl}${order.invoiceUrl}`}
                        target="_blank"
                        rel="noopener noreferrer" // security and privacy
                      >
                        Invoice
                      </a>
                    </td>

                    <td>
                      <button
                        className="btn btn-outline-success"
                        onClick={() => toggleForm(order.id)}
                      >
                        Finalizing
                      </button>
                      {visibleFormOrderId === order.id && (
                        <div className="mt-2">
                          <GreenForm orderId={order.id} />
                        </div>
                      )}
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          )}
        </Col>
      </Row>
    </Container>
  );
};

export default PendingOrders;
