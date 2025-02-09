import { useEffect, useState } from "react";
import { Card, CardContent } from "@/components/ui/card";

export default function UserDashboard() {
  const [reservas, setReservas] = useState([]);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const fetchReservations = () => {
    const token = localStorage.getItem("token");

    fetch(`http://localhost:5214/api/reservations?startDate=${startDate}&endDate=${endDate}`, {
      headers: { Authorization: `Bearer ${token}` },
    })
      .then((res) => res.json())
      .then((data) => setReservas(data))
      .catch(() => alert("Erro ao buscar reservas"));
  };

  return (
    <div className="flex justify-center items-center min-h-screen">
      <Card className="w-96 shadow-xl">
        <CardContent className="p-6">
          <h2 className="text-2xl font-bold text-center mb-4">Dashboard Usuário</h2>

          <label className="block mb-2">Data de Início:</label>
          <input type="date" className="w-full border p-2 rounded mb-4" value={startDate} onChange={(e) => setStartDate(e.target.value)} />

          <label className="block mb-2">Data de Fim:</label>
          <input type="date" className="w-full border p-2 rounded mb-4" value={endDate} onChange={(e) => setEndDate(e.target.value)} />

          <button className="w-full bg-blue-500 text-white p-2 rounded mb-4" onClick={fetchReservations}>Buscar Reservas</button>

          <h3 className="text-lg font-semibold mb-2">Suas Reservas:</h3>
          <ul className="list-disc pl-5">
            {reservas.map((reserva, index) => (
              <li key={index}>
                Suite: {reserva.suiteId} - {reserva.startDate} até {reserva.endDate}
              </li>
            ))}
          </ul>
        </CardContent>
      </Card>
    </div>
  );
}
