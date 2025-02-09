import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "../index.css"; // Importando os estilos

export default function Register() {
  const [name, setName] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault();

    const response = await fetch("http://localhost:5214/api/auth/register", { 
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        username: name,
        passwordHash: password,
        role: "User",
      }),
    });

    if (response.ok) {
      alert("Cadastro realizado com sucesso! Faça login.");
      navigate("/login");
    } else {
      const data = await response.json();
      alert(`Erro ao cadastrar: ${data.message || "Tente novamente."}`);
    }
  };

  return (
    <div className="auth-container">
      <h2>Cadastro</h2>
      <form onSubmit={handleRegister}>
        <label>Nome:</label>
        <input
          type="text"
          placeholder="Digite seu nome"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />

        <label>Senha:</label>
        <input
          type="password"
          placeholder="Digite sua senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <button type="submit">Cadastrar</button>
      </form>

      <p>Tem uma conta? <Link to="/login">Entre aqui</Link></p>
    </div>
  );
}
