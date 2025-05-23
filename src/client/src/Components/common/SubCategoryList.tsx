import { useEffect, useState } from "react";

import { CategoryWithSubCategories } from "../../types/SubCategoryTypes";
import { Link } from "react-router-dom";
import { categoriesService } from "../../services/categoriesService";
import { baseUrl } from "../../Constants/baseUrl";

interface Props {
  rootCategoryId: number;
}

const SubCategoriesList = ({ rootCategoryId }: Props) => {
  const [categoryData, setCategoryData] =
    useState<CategoryWithSubCategories | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchSubcategories = async () => {
      setLoading(true);
      try {
        const response = await categoriesService.getSubcategories(
          rootCategoryId
        );
        setCategoryData(response.data.data);
      } catch (err) {
        console.error("Failed to fetch subcategories:", err);
      }
      setLoading(false);
    };

    fetchSubcategories();
  }, [rootCategoryId]);

  if (loading) return <div>Loading subcategories...</div>;
  if (!categoryData) return null;

  return (
    <div className="card p-4 shadow-sm mb-4 border">
      <div className="d-flex">
        <div className="me-4 text-center" style={{ minWidth: "120px" }}>
          {categoryData.imagePath && (
            <img
              src={`${baseUrl}${categoryData.imagePath}`}
              alt={categoryData.name}
              style={{
                width: 100,
                height: 100,
                objectFit: "cover",
                borderRadius: 8,
                marginBottom: 10,
              }}
            />
          )}
          <h5>{categoryData.name}</h5>
        </div>
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(6, 1fr)",
            gap: "8px",
            width: "100%",
          }}
        >
          {categoryData.subCategories.map((sub) => (
            <div
              key={sub.id}
              className="border rounded p-3"
              style={{ backgroundColor: "rgba(255, 193, 7, 0.05)" }}
            >
              <Link
                to={`/subcategory/${sub.id}`}
                className="fw-bold text-dark text-center text-decoration-none"
              >
                {sub.name}
              </Link>
              <ul className="list-unstyled mb-0 mt-2">
                {sub.subSubCategories.map((subsub) => (
                  <li key={subsub.id}>
                    <Link
                      to={`/subcategory/${subsub.id}`}
                      className="text-dark text-decoration-none"
                    >
                      {subsub.name}
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};
export default SubCategoriesList;
