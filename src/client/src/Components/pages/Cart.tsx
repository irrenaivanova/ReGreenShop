import { useEffect, useCallback, useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { AllProductsInCart } from "../../types/AllProductsInCart";
import { cartService } from "../../services/cartService";
import { utilityService } from "../../services/utilityService";
import CartButton from "../common/CartButton";
import { ProductCategoryGroup } from "../../types/ProductCategoryGroup";
import { useProductActionsForCart } from "../../hooks/useProductActionsForCart";
import Spinner from "../common/Spinner";
import { baseUrl } from "../../Constants/baseUrl";

const Cart = () => {
  const navigate = useNavigate();
  const [cart, setCart] = useState<AllProductsInCart | null>(null);
  const [deliveryInfo, setDeliveryInfo] = useState<string[]>([]);

  const fetchCart = useCallback(async () => {
    const response = await cartService.viewProductsInCart();
    setCart(response.data.data);
  }, []);

  const fetchDeliveryPrices = async () => {
    const response = await utilityService.getAllDeliveryPrices();
    setDeliveryInfo(response.data);
  };

  useEffect(() => {
    fetchCart();
  }, [fetchCart]);

  const { handleIncrement, handleDecrement } =
    useProductActionsForCart(fetchCart);

  const handleClearCart = async () => {
    await cartService.cleanCart();
    fetchCart();
  };

  return (
    <div className="container my-4">
      <h4 className="mb-4 text-decoration-underline">Products in the Cart</h4>
      <div className="row">
        <div className="col-md-9">
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
                          className="d-flex align-items-center gap-3 justify-content-end"
                          style={{ fontSize: "1.1rem" }}
                        >
                          <div
                            className="d-flex flex-column"
                            style={{ minWidth: "160px" }}
                          >
                            <CartButton
                              quantity={product.quantityInCart || 0}
                              onIncrement={() => handleIncrement(product.id)}
                              onDecrement={() => handleDecrement(product.id)}
                              availableQuantity={product.stock}
                            />
                          </div>
                          <div className="d-flex gap-3 flex-grow-1 justify-content-end">
                            <p
                              className={`fw-bold mb-0 ${
                                product.hasPromoDiscount ||
                                product.hasTwoForOneDiscount
                                  ? "text-danger"
                                  : "text-dark"
                              }`}
                            >
                              {(product.hasTwoForOneDiscount
                                ? product.price
                                : product.discountPrice ?? product.price
                              ).toFixed(2)}{" "}
                              lv
                            </p>
                            <p className="fw-semibold mb-0">
                              {product.totalPriceProduct?.toFixed(2)} lv
                            </p>
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

        {/* Right Side */}
        <div className="col-md-3">
          {cart ? (
            <div className="card p-4">
              <h5 className="mb-3">Summary</h5>
              <p className="mb-1 fw-bold">
                Total Price: {(cart.totalPrice ?? 0).toFixed(2)} lv
              </p>
              <p className="mb-1 fw-bold">
                Delivery: {(cart.deliveryPriceProducts ?? 0).toFixed(2)} lv{" "}
                <span
                  className="badge bg-info text-dark"
                  style={{ cursor: "pointer" }}
                  onMouseEnter={fetchDeliveryPrices}
                  title={deliveryInfo.join("\n")}
                >
                  ?
                </span>
              </p>
              {cart.deliveryMessage && (
                <p className="text-muted small mt-2">{cart.deliveryMessage}</p>
              )}
              <button className="btn btn-danger mt-3" onClick={handleClearCart}>
                Clear Cart
              </button>
              <button
                className="btn btn-success mt-2"
                onClick={() => navigate("/checkout")}
              >
                Make an Order
              </button>
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
