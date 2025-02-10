import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import "../Pages/Home"; 

export default function Home() {
  const [moteis, setMoteis] = useState([]);

  useEffect(() => {
    async function fetchMoteis() {
      try {
        const response = await fetch("http://localhost:5214/api/Reservations");
        if (!response.ok) throw new Error("Erro ao buscar motéis");
        const data = await response.json();
        setMoteis(data);
      } catch (error) {
        console.error(error);
      }
    }

    fetchMoteis();
  }, []);

  return (
    <div className="home-container">
      {/* Navbar */}
      <nav className="navbar">
        <h1>Guia de Motéis</h1>
        <Link to="/login">
          <button>Entrar</button>
        </Link>
      </nav>

      {/* Hero Section */}
      <div className="hero-section">
        <h2>Bem-vindo ao Guia de Motéis</h2>
        <p>Encontre os melhores motéis da sua região.</p>
        <Link to="/login">
          <button>Explorar Agora</button>
        </Link>
      </div>

      {/* Lista de Motéis */}
      <div className="motel-list">
        {moteis.map((motel, index) => (
          <div key={index} className="motel-card">
            {/* <h3>{motel.nome}</h3>
            <p>{motel.endereco}</p> */}
          </div>
        ))}
      </div>

      {/* Footer */}
      <footer className="footer">
        <p>© 2024 Guia de Motéis. Todos os direitos reservados.</p>
      </footer>
    </div>
  );
}
