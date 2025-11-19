using APIAutoservice156.Models;
using Microsoft.EntityFrameworkCore;

namespace APIAutoservice156.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Users.Any() || context.Clients.Any() || context.Services.Any())
            {
                return;
            }

            var users = new User[]
            {
        new User {
            Username = "admin",
            Email = "admin@autoservice.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
            Role = "Admin"
        },
        new User {
            Username = "mechanic",
            Email = "mechanic@autoservice.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("mechanic123"),
            Role = "Mechanic"
        },
        new User {
            Username = "user",
            Email = "user@example.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
            Role = "User"
        }
            };
            context.Users.AddRange(users);
            context.SaveChanges();

            var clients = new Client[]
            {
                new Client{FirstName="Иван", LastName="Иванов", PhoneNumber="+79111111111", Email="ivan@mail.ru"},
                new Client{FirstName="Петр", LastName="Петров", PhoneNumber="+79222222222", Email="petr@mail.ru"},
                new Client{FirstName="Сергей", LastName="Сергеев", PhoneNumber="+79333333333", Email="sergey@mail.ru"},
                new Client{FirstName="Анна", LastName="Сидорова", PhoneNumber="+79444444444", Email="anna@mail.ru"},
                new Client{FirstName="Мария", LastName="Кузнецова", PhoneNumber="+79555555555", Email="maria@mail.ru"},
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();

            var vehicles = new Vehicle[]
            {
                new Vehicle{Brand="Toyota", Model="Camry", Year=2018, LicensePlate="А111АА777", ClientId=clients[0].Id},
                new Vehicle{Brand="Lada", Model="Vesta", Year=2020, LicensePlate="В222ВВ777", ClientId=clients[1].Id},
                new Vehicle{Brand="Kia", Model="Rio", Year=2019, LicensePlate="Е333ЕЕ777", ClientId=clients[2].Id},
                new Vehicle{Brand="Hyundai", Model="Solaris", Year=2017, LicensePlate="О444ОО777", ClientId=clients[3].Id},
                new Vehicle{Brand="BMW", Model="X5", Year=2015, LicensePlate="С555СС777", ClientId=clients[4].Id},
                new Vehicle{Brand="Toyota", Model="RAV4", Year=2021, LicensePlate="Т666ТТ777", ClientId=clients[0].Id},
            };
            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();

            var services = new Service[]
            {
                new Service{Name="Замена масла двигателя", Description="Полная замена моторного масла и масляного фильтра", Price=2000, DurationMinutes=30},
                new Service{Name="Замена воздушного фильтра", Description="Замена основного воздушного фильтра салона", Price=800, DurationMinutes=15},
                new Service{Name="Диагностика подвески", Description="Комплексная проверка элементов ходовой части", Price=1500, DurationMinutes=60},
                new Service{Name="Замена тормозных колодок", Description="Замена колодок одного моста", Price=3500, DurationMinutes=45},
                new Service{Name="Развал-схождение", Description="Регулировка углов установки колес", Price=2500, DurationMinutes=60},
            };
            context.Services.AddRange(services);
            context.SaveChanges();

            var appointments = new Appointment[]
            {
                new Appointment{AppointmentDateTime=DateTime.Now.AddDays(1), Status="Запланирован", VehicleId=vehicles[0].Id},
                new Appointment{AppointmentDateTime=DateTime.Now.AddDays(2), Status="Запланирован", VehicleId=vehicles[1].Id},
                new Appointment{AppointmentDateTime=DateTime.Now.AddDays(3), Status="Подтвержден", VehicleId=vehicles[2].Id},
                new Appointment{AppointmentDateTime=DateTime.Now.AddDays(1).AddHours(2), Status="Запланирован", VehicleId=vehicles[3].Id},
                new Appointment{AppointmentDateTime=DateTime.Now.AddDays(4), Status="Выполнен", VehicleId=vehicles[4].Id},
            };
            context.Appointments.AddRange(appointments);
            context.SaveChanges();

            var serviceAppointments = new ServiceAppointment[]
            {
                new ServiceAppointment{AppointmentId=appointments[0].Id, ServiceId=services[0].Id},
                new ServiceAppointment{AppointmentId=appointments[0].Id, ServiceId=services[1].Id},
                new ServiceAppointment{AppointmentId=appointments[1].Id, ServiceId=services[2].Id},
                new ServiceAppointment{AppointmentId=appointments[2].Id, ServiceId=services[3].Id},
                new ServiceAppointment{AppointmentId=appointments[3].Id, ServiceId=services[4].Id},
                new ServiceAppointment{AppointmentId=appointments[4].Id, ServiceId=services[0].Id},
                new ServiceAppointment{AppointmentId=appointments[4].Id, ServiceId=services[2].Id},
            };
            context.ServiceAppointments.AddRange(serviceAppointments);
            context.SaveChanges();

        }
    }
}