import { useEffect, useState } from "react";
import { useForm, Controller } from "react-hook-form";
import { utilityService } from "../../services/utilityService";
import { useModal } from "../../context/ModalContext";
import { Button, Col, Form, Row } from "react-bootstrap";
import Spinner from "./Spinner";
import { orderService } from "../../services/orderService";

interface GreenAlternative {
  id: number;
  name: string;
  maximumQuantity: number;
}

interface GreenFormProps {
  orderId: string;
  onFinish?: () => void;
}

type GreenModelInput = Record<string, number>;

const GreenForm = ({ orderId, onFinish }: GreenFormProps) => {
  const [alternatives, setAlternatives] = useState<GreenAlternative[]>([]);
  const [loading, setLoading] = useState(true);
  const { showModal } = useModal();

  const {
    control,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<GreenModelInput>();

  useEffect(() => {
    utilityService
      .getAllGreenAlternatives()
      .then((res) => setAlternatives(res.data.data))
      .finally(() => setLoading(false));
  }, []);

  const onSubmit = (data: GreenModelInput) => {
    const greenModels = Object.entries(data)
      .filter(([_, quantity]) => quantity > 0)
      .map(([id, quantity]) => ({
        id: parseInt(id),
        quantity,
      }));

    const payload = {
      orderId,
      greenModels,
    };
    try {
      orderService.finishAnOrder(payload);
      showModal?.("success", "Order successfully finished!");
      reset();

      if (onFinish) {
        onFinish(); //
      }
    } catch (error: any) {
      const errorMessage =
        error.response?.data?.message ||
        error.message ||
        "Failed to finish the order.";
      showModal?.("error", errorMessage);
    }

    reset();
  };
  if (loading) return <Spinner />;

  return (
    <Form onSubmit={handleSubmit(onSubmit)} className="mt-3">
      <Row>
        {[
          ...alternatives,
          { id: -1, name: "submit-button", maximumQuantity: 0 },
        ].map((alt, index) => (
          <Col md={6} key={alt.id !== -1 ? alt.id : "submit"} className="mb-3">
            {alt.id !== -1 ? (
              <Form.Group>
                <Form.Label>
                  {alt.name} (max {alt.maximumQuantity})
                </Form.Label>
                <Controller
                  name={`${alt.id}` as const}
                  control={control}
                  defaultValue={0}
                  rules={{
                    min: { value: 0, message: "Cannot be negative" },
                    max: {
                      value: alt.maximumQuantity,
                      message: `Max allowed is ${alt.maximumQuantity}`,
                    },
                  }}
                  render={({ field }) => (
                    <Form.Control
                      type="number"
                      {...field}
                      min={0}
                      max={alt.maximumQuantity}
                    />
                  )}
                />
                {errors[alt.id] && (
                  <Form.Text className="text-danger">
                    {errors[alt.id]?.message}
                  </Form.Text>
                )}
              </Form.Group>
            ) : (
              <div className="d-flex align-items-end h-100">
                <Button variant="primary" type="submit" className="w-100">
                  Submit
                </Button>
              </div>
            )}
          </Col>
        ))}
      </Row>
    </Form>
  );
};

export default GreenForm;
