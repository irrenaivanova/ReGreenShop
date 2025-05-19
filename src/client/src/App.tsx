import { useState } from 'react'
import './App.css'
import { Routes, Route } from 'react-router-dom';
import TopProducts from './Components/TopProducts';
import ProductDetails from './Components/ProductDetails';

function App() {

  return (
    <>
      <Routes>
      <Route path="/" element={<TopProducts/>}/>
      {/* <Route path="/products/:id" element={<ProductDetails />} /> */}
    </Routes>
    </>
  )
}

export default App
