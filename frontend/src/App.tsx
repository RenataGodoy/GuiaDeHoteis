import './App.css'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import Home from './Pages/Home'
import Dashboard from './Pages/dashboard'
import Register from './Pages/Register'
import Login from './Pages/Login'
import UserDashboard from './Pages/UserDashboard'
import AdminDashboard from './Pages/AdminDashboard'

function App() {

  return (
<BrowserRouter>
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/register" element={<Register />} />
      <Route path="/login" element={<Login />} />
      <Route path="/dashboard/:userRole" element={<Dashboard />}/>
      <Route path="/dashboard/User" element={<UserDashboard />} />
      <Route path="/dashboard/Admin" element={<AdminDashboard />} />
    </Routes>
  </BrowserRouter>
  )
}

export default App
