import { useEffect, useState } from "react";
import {
  Container,
  Row,
  Col,
  Table,
  Button,
  Modal,
  Pagination,
} from "react-bootstrap";
import { useModal } from "../../context/ModalContext";
import { AdminAllProduct } from "../../types/AdminAllProducts";
import { adminService } from "../../services/adminService";
import Spinner from "../common/Spinner";

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
  const { showModal } = useModal();

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

  useEffect(() => {
    fetchProducts();
  }, [page]);

  const openEditModal = (product: AdminAllProduct) => {
    setSelectedProduct(product);
    setShowEditModal(true);
  };

  const openDeleteModal = (product: AdminAllProduct) => {
    setSelectedProduct(product);
    setShowDeleteModal(true);
  };

  const handleDelete = async () => {
    // Call API to delete product here
    console.log("Deleting product ID:", selectedProduct?.id);
    setShowDeleteModal(false);
    await fetchProducts();
  };

  const handleEditSave = async () => {
    // Save changes logic here
    console.log("Saving edits for:", selectedProduct?.id);
    setShowEditModal(false);
    await fetchProducts();
  };

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
      <Row className="justify-content-center">
        <Col md={11}>
          <h4 className="mb-4 text-primary">All Products</h4>

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
                  {products.map((product, index) => (
                    <tr key={product.id}>
                      <td>{(page - 1) * PAGE_SIZE + index + 1}</td>
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
                      <td>
                        <Button
                          variant="outline-primary"
                          size="sm"
                          className="me-2"
                          onClick={() => openEditModal(product)}
                        >
                          Edit
                        </Button>
                        <Button
                          variant="outline-danger"
                          size="sm"
                          onClick={() => openDeleteModal(product)}
                        >
                          Delete
                        </Button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </Table>

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
            </>
          )}
        </Col>
      </Row>

      {/* Edit Modal */}
      <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Edit Product</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <p>
            Edit form for <strong>{selectedProduct?.name}</strong> goes here.
          </p>
          {/* Replace with real form */}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowEditModal(false)}>
            Cancel
          </Button>
          <Button variant="primary" onClick={handleEditSave}>
            Save Changes
          </Button>
        </Modal.Footer>
      </Modal>

      {/* Delete Modal */}
      <Modal show={showDeleteModal} onHide={() => setShowDeleteModal(false)}>
        <Modal.Header closeButton>
          <Modal.Title>Delete Product</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          Are you sure you want to delete{" "}
          <strong>{selectedProduct?.name}</strong>?
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={() => setShowDeleteModal(false)}>
            Cancel
          </Button>
          <Button variant="danger" onClick={handleDelete}>
            Delete
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default AllProductsPaginated;
