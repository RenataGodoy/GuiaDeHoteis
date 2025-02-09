import { useEffect, useState } from "react";
import { Card, CardContent } from "@/components/ui/card";

export default function AdminDashboard() {
  const [faturamento, setFaturamento] = useState<number | null>(null);
  const [mes, setMes] = useState<string>(String(new Date().getMonth() + 1));
  const [ano, setAno] = useState<string>(String(new Date().getFullYear()));

  useEffect(() => {
    async function fetchFaturamento() {
      const token = localStorage.getItem("token");

      if (!token) {
        alert("Usuário não autenticado!");
        return;
      }

      try {
        const response = await fetch(
          `http://localhost:5214/api/Reservation/monthly-revenue?month=${mes}&year=${ano}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );

        console.log("🟡 Status da resposta:", response.status);

        if (!response.ok) {
          const errorText = await response.text();
          throw new Error(`Erro na API: ${response.status} - ${errorText}`);
        }

        const data = await response.json();
        console.log("🟢 Resposta da API:", data);

        if (typeof data.revenue === "number") {
          setFaturamento(data.revenue);
        } else {
          throw new Error("Resposta da API não contém um número válido");
        }
      } catch (error) {
        console.error("🔴 Erro ao buscar faturamento:", error);
        alert("Erro ao buscar faturamento. Verifique o console para mais detalhes.");
      }
    }

    fetchFaturamento();
  }, [mes, ano]);

  return (
    <div className="flex justify-center items-center min-h-screen">
      <Card className="w-96 shadow-xl">
        <CardContent className="p-6">
          <h2 className="text-2xl font-bold text-center mb-4">Dashboard do Admin</h2>

          <label htmlFor="mes">Mês:</label>
          <select id="mes" value={mes} onChange={(e) => setMes(e.target.value)} className="w-full border p-2 rounded mb-4">
            {Array.from({ length: 12 }, (_, i) => (
              <option key={i + 1} value={String(i + 1)}>
                {new Date(2022, i).toLocaleString("pt-BR", { month: "long" })}
              </option>
            ))}
          </select>

          <label htmlFor="ano">Ano:</label>
          <select id="ano" value={ano} onChange={(e) => setAno(e.target.value)} className="w-full border p-2 rounded mb-4">
            {Array.from({ length: 5 }, (_, i) => {
              const year = new Date().getFullYear() - i;
              return <option key={year} value={String(year)}>{year}</option>;
            })}
          </select>

          <p className="text-center text-lg">
            <strong>Faturamento mensal:</strong> R$ {faturamento !== null ? faturamento.toLocaleString("pt-BR", { minimumFractionDigits: 2 }) : "Carregando..."}
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
