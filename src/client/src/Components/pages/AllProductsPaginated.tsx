import { useEffect, useRef, useState } from "react";
import {
  Container,
  Row,
  Col,
  Table,
  Button,
  Pagination,
  Form,
} from "react-bootstrap";
import { useModal } from "../../context/ModalContext";
import { AdminAllProduct } from "../../types/AdminAllProducts";
import { adminService } from "../../services/adminService";
import Spinner from "../common/Spinner";
import { TiDeleteOutline } from "react-icons/ti";
import { CiEdit } from "react-icons/ci";
import { baseUrl } from "../../Constants/baseUrl";
import ProductEditModal from "../common/ProductEditModal";
import ProductDeleteModal from "../common/ProductDeleteModel";

const PAGE_SIZE = 10;

const AllProductsPaginated = () => {
  const [products, setProducts] = useState<AdminAllProduct[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedProduct, setSelectedProduct] =
    useState<AdminAllProduct | null>(null);
  const [showEditModal, setShowEditModal] = useState(false);
  const [showDeleteModal, setShowDeleteModal] = useState(false);
  const [editStock, setEditStock] = useState<number>(0);
  const [editPrice, setEditPrice] = useState<number>(0);
  const { showModal } = useModal();
  const [searchFilters, setSearchFilters] = useState<{
    searchString?: string;
    minPrice?: string;
    maxPrice?: string;
    minStock?: number;
    maxStock?: number;
  } | null>(null);

  const fetchProducts = async () => {
    setIsLoading(true);
    try {
      const response = await adminService.getAllProducts({
        page,
        pageSize: PAGE_SIZE,
      });
      const data = response.data.data;
      setProducts(data.products);
      setTotalPages(data.totalPages);
    } catch (err: any) {
      const error = err.response?.data?.error || "Failed to load products.";
      showModal?.("error", error);
    } finally {
      setIsLoading(false);
    }
  };

  const fetchSearchedProducts = async () => {
    setIsLoading(true);
    try {
      const response = await adminService.searchAllProducts({
        page,
        pageSize: PAGE_SIZE,
        ...searchFilters,
      });
      const data = response.data.data;
      setProducts(data.products);
      setTotalPages(data.totalPages);
    } catch (err: any) {
      const error =
        err.response?.data?.error || "Failed to load search results.";
      showModal?.("error", error);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    if (searchFilters) {
      fetchSearchedProducts();
    } else {
      fetchProducts();
    }
  }, [page, searchFilters]);

  const openEditModal = (product: AdminAllProduct) => {
    setSelectedProduct(product);
    setEditStock(product.stock);
    setEditPrice(product.price);
    setShowEditModal(true);
  };

  const openDeleteModal = (product: AdminAllProduct) => {
    setSelectedProduct(product);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    if (!selectedProduct) return;

    try {
      await adminService.deleteProduct(selectedProduct.id);
      showModal?.("success", "Product deleted successfully.");
      setShowDeleteModal(false);
      await fetchProducts();
    } catch (error: any) {
      const errMessage = error.response?.data?.error || "Delete failed.";
      showModal?.("error", errMessage);
    }
  };

  const handleEditSave = async () => {
    if (!selectedProduct) return;

    try {
      await adminService.updateProduct({
        id: selectedProduct.id,
        price: editPrice,
        stock: editStock,
      });
      showModal?.("success", "Product updated successfully.");
      setShowEditModal(false);
      fetchProducts();
    } catch (error: any) {
      const errMessage = error.response?.data?.error || "Update failed.";
      showModal?.("error", errMessage);
    }
  };

  const formRef = useRef<HTMLFormElement>(null);

  const renderPaginationItems = () => {
    const pages = [];
    const leftBound = Math.max(2, page - 2);
    const rightBound = Math.min(totalPages - 1, page + 2);

    pages.push(
      <Pagination.Item key={1} active={page === 1} onClick={() => setPage(1)}>
        1
      </Pagination.Item>
    );

    if (leftBound > 2) {
      pages.push(<Pagination.Ellipsis key="left-ellipsis" disabled />);
    }

    for (let p = leftBound; p <= rightBound; p++) {
      pages.push(
        <Pagination.Item key={p} active={p === page} onClick={() => setPage(p)}>
          {p}
        </Pagination.Item>
      );
    }

    if (rightBound < totalPages - 1) {
      pages.push(<Pagination.Ellipsis key="right-ellipsis" disabled />);
    }

    if (totalPages > 1) {
      pages.push(
        <Pagination.Item
          key={totalPages}
          active={page === totalPages}
          onClick={() => setPage(totalPages)}
        >
          {totalPages}
        </Pagination.Item>
      );
    }

    return pages;
  };

  return (
    <Container className="my-4">
      <Form
        ref={formRef}
        className="mb-3 col-md-12 "
        onSubmit={(e) => {
          e.preventDefault();
          const form = e.currentTarget;
          const formData = new FormData(form);

          const newFilters = {
            searchString: formData.get("searchString")?.toString() || undefined,
            minPrice: formData.get("minPrice")?.toString() || undefined,
            maxPrice: formData.get("maxPrice")?.toString() || undefined,
            minStock: Number(formData.get("minStock")) || undefined,
            maxStock: Number(formData.get("maxStock")) || undefined,
          };

          setPage(1);
          setSearchFilters(newFilters);
        }}
      >
        <Row>
          <Col md={3}>
            <Form.Control name="searchString" placeholder="Search product..." />
          </Col>
          <Col md={2}>
            <Form.Control
              name="minPrice"
              type="string"
              placeholder="Min price"
            />
          </Col>
          <Col md={2}>
            <Form.Control
              name="maxPrice"
              type="string"
              placeholder="Max price"
            />
          </Col>
          <Col md={2}>
            <Form.Control
              name="minStock"
              type="number"
              placeholder="Min stock"
            />
          </Col>
          <Col md={2}>
            <Form.Control
              name="maxStock"
              type="number"
              placeholder="Max stock"
            />
          </Col>
          <Col md={1}>
            <Button type="submit" variant="primary">
              Search
            </Button>
          </Col>
          <Col md={1}>
            <Button
              type="button"
              className="mt-2"
              variant="outline-secondary"
              onClick={() => {
                setSearchFilters(null);
                setPage(1);

                formRef.current?.reset();
              }}
            >
              Clear
            </Button>
          </Col>
        </Row>
      </Form>

      <Row className="justify-content-center">
        <Col md={12}>
          <h4 className="mb-4 text-primary text-center text-decoration-underline">
            All Products
          </h4>
          <div className="d-flex justify-content-center">
            <Pagination>
              <Pagination.Prev
                onClick={() => setPage(page - 1)}
                disabled={page === 1}
              />
              {renderPaginationItems()}
              <Pagination.Next
                onClick={() => setPage(page + 1)}
                disabled={page === totalPages}
              />
            </Pagination>
          </div>
          {isLoading ? (
            <div className="text-center py-5">
              <Spinner />
            </div>
          ) : products.length === 0 ? (
            <div className="text-center text-muted fs-5">
              No products available.
            </div>
          ) : (
            <>
              <Table striped bordered hover responsive>
                <thead className="table-primary">
                  <tr>
                    <th>#</th>
                    <th>Image</th>
                    <th>Name</th>
                    <th>Price</th>
                    <th>Packaging</th>
                    <th>Code</th>
                    <th>Brand</th>
                    <th>Origin</th>
                    <th>%</th>
                    <th>Stock</th>
                    <th>Labels</th>
                    <th>Categories</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {products.map((product, index) => {
                    return (
                      <tr key={product.id}>
                        <td>{(page - 1) * PAGE_SIZE + index + 1}</td>
                        <td>
                          <img
                            src={baseUrl + product.imagePath}
                            alt="Product"
                            className="img-fluid d-block mx-auto"
                            style={{
                              maxWidth: "50px",
                              maxHeight: "50px",
                              objectFit: "contain",
                            }}
                          />
                        </td>
                        <td>{product.name}</td>
                        <td>{product.price.toFixed(2)}</td>
                        <td>{product.packaging}</td>
                        <td>{product.productCode}</td>
                        <td>{product.brand}</td>
                        <td>{product.origin}</td>
                        <td>{product.discountPercentage ?? "-"}</td>
                        <td>{product.stock}</td>
                        <td>{product.labels.join(", ")}</td>
                        <td>{product.categories.join(", ")}</td>
                        <td className="text-center">
                          <span
                            onClick={() => openEditModal(product)}
                            className="text-primary me-3"
                            style={{ cursor: "pointer" }}
                          >
                            <CiEdit size={18} />
                          </span>
                          <span
                            onClick={() => openDeleteModal(product)}
                            className="text-danger"
                            style={{ cursor: "pointer" }}
                          >
                            <TiDeleteOutline size={18} />
                          </span>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </Table>
            </>
          )}
        </Col>
      </Row>

      <ProductEditModal
        show={showEditModal}
        onHide={() => setShowEditModal(false)}
        onSave={handleEditSave}
        price={editPrice}
        stock={editStock}
        onChange={(field, value) => {
          if (field === "price") setEditPrice(value);
          else if (field === "stock") setEditStock(value);
        }}
      />
      <ProductDeleteModal
        show={showDeleteModal}
        onHide={() => setShowDeleteModal(false)}
        onDelete={handleDelete}
        productName={selectedProduct?.name}
      />
    </Container>
  );
};

export default AllProductsPaginated;
