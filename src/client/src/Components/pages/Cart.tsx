import { useEffect, useCallback, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { AllProductsInCart } from "../../types/AllProductsInCart";
import { cartService } from "../../services/cartService";
import { utilityService } from "../../services/utilityService";
import CartButton from "../common/CartButton";
import { ProductCategoryGroup } from "../../types/ProductCategoryGroup";
import Spinner from "../common/Spinner";
import { baseUrl } from "../../Constants/baseUrl";
import { useCart } from "../../context/CartContext";
import { useProductActionsForCart } from "../../hooks/useProductActionsForCart";
import { FaTrashAlt, FaTruck } from "react-icons/fa";
import DeliveryInfo from "../common/DeliveryInfo";
import { Delivery } from "../../types/Delivery";
import { useAuth } from "../../context/AuthContext";
import { Button } from "react-bootstrap";
import { BsXCircleFill } from "react-icons/bs";

const Cart = () => {
  const navigate = useNavigate();
  const [cart, setCart] = useState<AllProductsInCart | null>(null);
  const [deliveryInfo, setDeliveryInfo] = useState<Delivery[]>([]);
  const [showPopup, setShowPopup] = useState(false);
  const { isAuthenticated } = useAuth();
  const [showTooltip, setShowTooltip] = useState(false);

  const fetchCart = useCallback(async () => {
    const response = await cartService.viewProductsInCart();
    setCart(response.data.data);
  }, []);

  const fetchDeliveryPrices = async () => {
    const response = await utilityService.getAllDeliveryPrices();
    setDeliveryInfo(response.data.data);
  };

  const { refreshCartCount } = useCart();

  useEffect(() => {
    fetchCart();
  }, [fetchCart]);

  const { handleIncrement, handleDecrement } =
    useProductActionsForCart(fetchCart);

  const handleClearCart = async () => {
    await cartService.cleanCart();
    await refreshCartCount();
    fetchCart();
  };
  const handleRemoveProduct = async (productId: number) => {
    try {
      await cartService.removeAllProducts(productId);
      fetchCart();
      await refreshCartCount();
    } catch (error) {}
  };

  const isDisabled = !isAuthenticated || (cart?.totalPrice ?? 0) < 40;
  return (
    <div className="container my-4">
      <div className="row">
        <div className="col-12 col-md-9">
          <div className="d-flex justify-content-between align-items-center mb-4">
            <h4 className="mb-0">Products in Cart</h4>
            <span
              onClick={handleClearCart}
              style={{ cursor: "pointer", textDecoration: "underline" }}
              className="text-danger d-flex align-items-center gap-2"
            >
              <FaTrashAlt />
              Clear Cart
            </span>
          </div>
          {cart?.productsByCategories?.length ? (
            cart.productsByCategories.map((group: ProductCategoryGroup) => (
              <div key={group.id} className="mb-4">
                <h5 className="fw-bold border-bottom pb-3">
                  <Link
                    to={`/category/${group.id}`}
                    className="text-decoration-none text-dark"
                  >
                    {group.categoryName}
                  </Link>
                </h5>
                <div>
                  {group.products.map((product) => (
                    <div key={product.id} className="mb-3 pb-3">
                      <div className="d-flex justify-content-between align-items-start gap-3">
                        <div className="d-flex gap-3 align-items-start">
                          <img
                            src={`${baseUrl}${product.imagePath}`}
                            alt={product.name}
                            style={{
                              width: 70,
                              height: 70,
                              objectFit: "contain",
                            }}
                          />
                          <div>
                            <h6 className="mb-1">{product.name}</h6>
                            <div className="d-flex gap-2 flex-wrap">
                              <span className="badge bg-secondary text-white">
                                {product.packaging}
                              </span>
                              {product.hasTwoForOneDiscount && (
                                <span className="badge bg-danger text-light">
                                  Two for One
                                </span>
                              )}
                              {product.hasPromoDiscount && (
                                <span className="badge bg-danger text-light">
                                  Promo
                                </span>
                              )}
                            </div>
                          </div>
                        </div>

                        <div
                          className="d-flex align-items-center gap-3"
                          style={{ fontSize: "1.1rem", width: "280px" }}
                        >
                          <div
                            className="d-flex align-items-start"
                            style={{ fontSize: "1.1rem" }}
                          >
                            <div className="d-flex" style={{ flexShrink: 0 }}>
                              <CartButton
                                quantity={product.quantityInCart || 0}
                                onIncrement={() => handleIncrement(product.id)}
                                onDecrement={() => handleDecrement(product.id)}
                                availableQuantity={product.stock}
                              />
                            </div>
                          </div>
                          <div className="d-flex gap-3 flex-grow-1 justify-content-end">
                            <p
                              className={`mb-0 ${
                                product.hasPromoDiscount ||
                                product.hasTwoForOneDiscount
                                  ? "text-danger"
                                  : "text-black"
                              }`}
                            >
                              {(product.hasTwoForOneDiscount
                                ? product.price
                                : product.discountPrice ?? product.price
                              ).toFixed(2)}{" "}
                              lv
                            </p>
                            <p className="fw-bold mb-0">
                              {product.totalPriceProduct?.toFixed(2)} lv
                            </p>
                            <Button
                              variant="outline-danger"
                              size="sm"
                              className="p-0"
                              style={{
                                border: "none",
                                background: "transparent",
                              }}
                              onClick={() => handleRemoveProduct(product.id)}
                              title="Remove from cart"
                            >
                              <BsXCircleFill size={18} />
                            </Button>
                          </div>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            ))
          ) : (
            <p>Your cart is empty.</p>
          )}
        </div>

        <div className="col-md-3">
          {cart ? (
            <div className="card p-4">
              <h5 className="mb-3">Summary</h5>
              <div className="d-flex justify-content-between mb-2">
                <span className="text-muted">Total Price:</span>
                <span className="fw-bold">
                  {(cart.totalPrice ?? 0).toFixed(2)} lv
                </span>
              </div>
              <div className="d-flex justify-content-between mb-2">
                <span className="text-muted">Delivery:</span>
                <span className="fw-bold">
                  {(cart.deliveryPriceProducts ?? 0).toFixed(2)}lv
                </span>
              </div>
              <div className="d-flex justify-content-between border-top pt-2 mt-2">
                <span className="text-dark">Grand Total:</span>
                <span className="fw-bold text-dark">
                  {(
                    (cart.deliveryPriceProducts ?? 0) + (cart.totalPrice ?? 0)
                  ).toFixed(2)}{" "}
                  lv
                </span>
              </div>
              <div
                className="d-flex align-items-center justify-content-between bg-white text-primary border border-primary rounded px-3 py-2 mt-3"
                style={{ position: "relative" }}
              >
                <div className="d-flex align-items-center gap-2">
                  <FaTruck size={20} style={{ flexShrink: 0 }} />
                  <span className="small">{cart.deliveryMessage}</span>
                </div>

                <span
                  className="badge bg-warning text-dark ms-3"
                  style={{ cursor: "pointer" }}
                  onMouseEnter={() => {
                    fetchDeliveryPrices();
                    setShowPopup(true);
                  }}
                  onMouseLeave={() => setShowPopup(false)}
                >
                  ?
                </span>

                {showPopup && <DeliveryInfo info={deliveryInfo} />}
              </div>
              <div className="position-relative d-inline-flex align-items-center gap-2 mt-2">
                <button
                  className={`btn btn-primary btn-lg ${
                    isDisabled ? "disabled" : ""
                  }`}
                  disabled={isDisabled}
                  onClick={() => {
                    if (!isDisabled)
                      navigate("/checkout", {
                        state: {
                          totalPrice:
                            (cart.deliveryPriceProducts ?? 0) +
                            (cart.totalPrice ?? 0),
                        },
                      });
                  }}
                >
                  Make an Order
                </button>

                {!isAuthenticated && (
                  <span
                    className="badge bg-warning text-dark"
                    style={{
                      cursor: "pointer",
                      lineHeight: 1,
                      padding: "0.35em 0.5em",
                    }}
                    onMouseEnter={() => setShowTooltip(true)}
                    onMouseLeave={() => setShowTooltip(false)}
                  >
                    ?
                  </span>
                )}

                {showTooltip && !isAuthenticated && (
                  <div
                    className="position-absolute bg-light border rounded p-2 shadow text-dark small"
                    style={{
                      top: "100%",
                      left: "50%",
                      transform: "translateX(-50%)",
                      marginTop: 6,
                      whiteSpace: "normal",
                      minWidth: 180,
                      zIndex: 1000,
                    }}
                  >
                    You must be logged in to make an order.
                  </div>
                )}
              </div>
            </div>
          ) : (
            <Spinner />
          )}
        </div>
      </div>
    </div>
  );
};
export default Cart;
