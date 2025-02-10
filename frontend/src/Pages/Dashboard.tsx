import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const Dashboard = () => {
  const navigate = useNavigate();
  const [userRole, setUserRole] = useState("");

  useEffect(() => {
    const role = localStorage.getItem("userRole");
    if (role) {
      setUserRole(role);
    } else {
      navigate("/login"); // Redireciona para login se não tiver um userRole
    }
  }, [navigate]);

  useEffect(() => {
    if (userRole) {
      if (userRole === "User") {
        navigate("/dashboard/User"); // Redireciona para UserDashboard
      } else if (userRole === "Admin") {
        navigate("/dashboard/Admin"); // Redireciona para AdminDashboard
      }
    }
  }, [userRole, navigate]);

  return <div>Carregando...</div>; // Exibe "Carregando..." até redirecionar
};

export default Dashboard;
