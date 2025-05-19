import { useState } from 'react'
import './App.css'
import Logo from './Components/Logo'
import CategoryList from './Components/CategoryList'
import ReGreenMission from './Components/ReGreenMission'
import Navbar from './Components/Navbar'

function App() {

  return (
    <>
    <ReGreenMission />
    <div><Logo /></div>
    <CategoryList />
    <Navbar/>

    </>
  )
}

export default App
