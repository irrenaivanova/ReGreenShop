import React, { useEffect, useState } from 'react';
import axios from 'axios';

interface Category {
    id: number;
    name: string;
    imagePath: string;
  }

  interface Label {
    id: number;
    name: string;
  }

const BottomNavbar = () => {
    const [categories, setCategories] = useState<Category[]>([]);
    const [labels, setLabels] = useState<Label[]>([]);

    useEffect(() => {
        axios.get('/api/categories/root').then(res => setCategories(res.data));
        axios.get('/api/labels').then(res => setLabels(res.data));
      }, []);
    
  return (
    <div>
      
    </div>
  )
}

export default BottomNavbar
