using GuiaDeMoteisAPI.Data;
using GuiaDeMoteisAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuiaDeMoteisAPI.Seed
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Verifica se o banco de dados foi criado
            context.Database.EnsureCreated();

            // Verifica se já existem dados na tabela Users
            if (context.Users.Any())
            {
                return;   // O banco já está populado
            }

            // Adiciona usuários de exemplo
            var users = new List<User>
            {
                new User { Username = "user1", PasswordHash = "password1", Role = "User" },
                new User { Username = "user2", PasswordHash = "password2", Role = "User" },
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            // Adiciona clientes de exemplo
            var clients = new List<Client>
            {
                new Client { Name = "Client 1", Email = "client1@example.com" },
                new Client { Name = "Client 2", Email = "client2@example.com" },
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();

            // Adiciona motéis e suítes de exemplo
            var motels = new List<Motel>
            {
                new Motel { Name = "Motel 1", Location = "Location 1" },
                new Motel { Name = "Motel 2", Location = "Location 2" }
            };
            context.Motels.AddRange(motels);
            context.SaveChanges();

            var suites = new List<Suite>
            {
                new Suite { Name = "Suite 1", MotelId = motels[0].Id, Price = 100 },
                new Suite { Name = "Suite 2", MotelId = motels[1].Id, Price = 150 }
            };
            context.Suites.AddRange(suites);
            context.SaveChanges();

            // Adiciona reservas de exemplo
            var reservations = new List<Reservation>
            {
                new Reservation { ClientId = clients[0].Id, SuiteId = suites[0].Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TotalAmount = 100 },
                new Reservation { ClientId = clients[1].Id, SuiteId = suites[1].Id, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), TotalAmount = 150 }
            };
            context.Reservations.AddRange(reservations);
            context.SaveChanges();
        }
    }
}
