import { useState } from "react";
import "../index.css";

export default function UserDashboard() {
  const [reservas, setReservas] = useState([]);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const formatDate = (date: string | Date): string => {
    if (!date) return "";
    return new Date(date).toISOString().split("T")[0];
  };

  const fetchReservations = () => {
    if (!startDate || !endDate) {
      setError("Selecione ambas as datas antes de buscar reservas.");
      return;
    }

    setLoading(true);
    setError("");
    const token = localStorage.getItem("token");

    if (!token) {
      setError("Token de autenticação não encontrado.");
      setLoading(false);
      return;
    }

    const formattedStartDate = formatDate(startDate);
    const formattedEndDate = formatDate(endDate);

    fetch(`http://localhost:5214/api/Reservation/reservations?startDate=${formattedStartDate}&endDate=${formattedEndDate}`, {
        headers: { Authorization: `Bearer ${token}` },
    })
      .then((res) => {
        if (!res.ok) {
          throw new Error("Erro ao buscar reservas");
        }
        return res.json();
      })
      .then((data) => {
        setReservas(data);
        setLoading(false);
      })
      .catch((err) => {
        setError(err.message || "Erro ao buscar reservas");
        setLoading(false);
      });
  };

  return (
    <div className="auth-container">
      <h2>Dashboard do Usuário</h2>

      <label>Data de Início:</label>
      <input type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />

      <label>Data de Fim:</label>
      <input type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />

      <button onClick={fetchReservations} disabled={loading}>
        {loading ? "Carregando..." : "Buscar Reservas"}
      </button>

      {error && <div className="error-message">{error}</div>}

      <h3>Todas reservas:</h3>
      {reservas.length === 0 ? (
        <p>Nenhuma reserva encontrada.</p>
      ) : (
                <pre>
        {JSON.stringify(
            reservas.map(({ startDate, endDate, totalAmount }) => ({
            startDate,
            endDate,
            totalAmount,
            })),
            null,
            2
        )}
        </pre>

      )}
    </div>
  );
}
