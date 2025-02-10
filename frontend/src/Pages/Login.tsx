import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import "../index.css"; // Importando os estilos

export default function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("");
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();

    const response = await fetch("http://localhost:5214/api/auth/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        username,
        passwordHash: password,
        role,
      }),
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem("token", data.token);
      localStorage.setItem("userRole", role); // Armazenando o role no localStorage
      navigate(`/dashboard/${role}`); // Redireciona para o painel do usuário com base no role
    } else {
      alert("Login falhou! Verifique suas credenciais.");
    }
  };

  return (
    <div className="auth-container">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <label>Nome de Usuário:</label>
        <input
          type="text"
          placeholder="Digite seu nome de usuário"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />

        <label>Senha:</label>
        <input
          type="password"
          placeholder="Digite sua senha"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />

        <label>Escolha o Tipo de Login:</label>
        <select value={role} onChange={(e) => setRole(e.target.value)}>
          <option value="User">Usuário</option>
          <option value="Admin">Administrador</option>
        </select>

        <button type="submit">Entrar</button>
      </form>

      <p>Não tem uma conta? <Link to="/register">Cadastre-se aqui</Link></p>
    </div>
  );
}
