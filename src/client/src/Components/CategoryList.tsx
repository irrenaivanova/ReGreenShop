import React, { useEffect, useState } from 'react';
import axios from 'axios';

interface Category {
  id: number;
  name: string;
  imagePath: string;
}

const CategoryList: React.FC = () => {
  const [categories, setCategories] = useState<Category[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get('https://localhost:7102/api/Category/GetRootCategories');
        
        if (!Array.isArray(response.data)) {
          throw new Error("Unexpected response type");
        }
  
        setCategories(response.data);
      } catch (err) {
        console.error(err);
        setError('Failed to load categories');
      } finally {
        setLoading(false);
      }
    };
  
    fetchCategories();
  }, []);

  if (loading) return <div>Loading categories...</div>;
  if (error) return <div>{error}</div>;

  return (
    <div className="category-list">
      <h2>Main Categories</h2>
      {categories.length === 0 ? (
        <p>No categories found.</p>
      ) : (
        <ul>
          {categories.map((category) => (
            <li key={category.id}>
              <img
                src={`https://localhost:7102/${category.imagePath}.png`}
                alt={category.name}
                style={{ width: 50, height: 50, objectFit: 'cover', marginRight: 10 }}
              />
              {category.name}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default CategoryList;