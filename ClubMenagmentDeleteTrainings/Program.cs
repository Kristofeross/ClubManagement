using ClubManagement.ApplicationDbContext;
using ClubManagement.Models;
using ClubManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Quartz.Impl;
using Quartz;
using System.Threading.Tasks;

namespace ClubMenagmentDeleteTrainings
{
    class Program
    {
        // Wykorzytaniem timera czy coś takiego
        /*private static Timer _timer;
        static void Main(string[] args)
        {

            *//*
            TimeSpan interval = TimeSpan.FromMinutes(5);
            //TimeSpan interval = TimeSpan.FromHours(1);
            _timer = new Timer(RemoveOutdatedTrainings, null, TimeSpan.Zero, interval);
            *//*

            // Obliczanie czasu do najbliższego wykonania o konkretnej godzinie
            DateTime now = DateTime.Now;
            DateTime nextRunTime = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0); // np. 1:00 AM
            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1); // Jeśli już minęła określona godzina, to ustaw na następny dzień
            }
            TimeSpan timeToNextRun = nextRunTime - now;

            // Uruchamiamy metodę do usuwania treningów o określonej godzinie
            _timer = new Timer(RemoveOutdatedTrainings, null, timeToNextRun, TimeSpan.FromHours(24)); // Wykonanie co 24 godziny



            Console.WriteLine("Aplikacja uruchomiona. Naciśnij klawisz 'q' aby zakończyć.");

            // Oczekiwanie na naciśnięcie klawisza 'q' do zakończenia działania aplikacji
            while (Console.ReadKey().Key != ConsoleKey.Q) ;
        }*/

        // Z wykorzystaniem Quartz
        static async Task Main(string[] args)
        {
            var scheduler = new TrainingCleanupScheduler();
            await scheduler.Start();

            Console.WriteLine("Aplikacja została uruchomiona. Zadanie usuwania treningów zostanie wykonane cyklicznie.");
            Console.ReadLine(); // Zatrzymujemy aplikację, aby nie zakończyła się zbyt szybko
        }



        // Metoda do timera
        /*static void RemoveOutdatedTrainings(object state)
        {
            // Konfiguracja konfiguracji aplikacji z appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Pobierz connection string z konfiguracji
            string connectionString = configuration.GetConnectionString("con");

            // Konfiguracja DbContextu
            var optionsBuilder = new DbContextOptionsBuilder<ClubDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var dbContext = new ClubDbContext(optionsBuilder.Options))
            {
                // Pobierz przeterminowane treningi indywidualne
                var outdatedIndividualTrainings = dbContext.IndividualTraining
                    .Where(it => it.EndTraining < DateTime.Now)
                    .ToList();

                // Usuń przeterminowane treningi indywidualne
                dbContext.IndividualTraining.RemoveRange(outdatedIndividualTrainings);

                // Pobierz przeterminowane treningi grupowe
                var outdatedGroupTrainings = dbContext.GroupTraining
                    .Where(gt => gt.EndTraining < DateTime.Now)
                    .ToList();

                // Usuń przeterminowane treningi grupowe
                dbContext.GroupTraining.RemoveRange(outdatedGroupTrainings);

                // Zapisz zmiany w bazie danych
                dbContext.SaveChanges();

                Console.WriteLine($"Usunięto {outdatedIndividualTrainings.Count} przeterminowanych treningów indywidualnych.");
                Console.WriteLine($"Usunięto {outdatedGroupTrainings.Count} przeterminowanych treningów grupowych.");
            }
        }*/
    }
    // Klasy do quartz
    public class TrainingCleanupScheduler
    {
        public async Task Start()
        {
            // Tworzymy harmonogram Quartz.NET
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            // Tworzymy zadanie usuwania treningów
            IJobDetail job = JobBuilder.Create<TrainingCleanupJob>()
                .WithIdentity("trainingCleanupJob", "group1")
                .Build();

            // Określamy godzinę, o której chcemy uruchomić zadanie
            //DateTime startTime = DateTime.Today.AddHours(13); // Uruchomienie o 13:00

            // Jeśli godzina już minęła, ustawiamy na następny dzień

            /*if (DateTime.Now > startTime)
            {
                startTime = startTime.AddDays(1);
            }*/

            // Tworzymy wyzwalacz, który uruchamia zadanie co 5 minut

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trainingCleanupTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(5) // Zmiana interwału na 5 minut
                    .RepeatForever())
                .Build();

            // Tworzymy wyzwalacz, który uruchamia zadanie o określonej godzinie
            /*ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trainingCleanupTrigger", "group1")
                .StartAt(startTime) // Uruchomienie o określonej godzinie
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24) // Powtarzamy co 24 godziny
                    .RepeatForever())
                .Build();*/

            // Harmonogramujemy zadanie
            await scheduler.ScheduleJob(job, trigger);
        }
    }

    public class TrainingCleanupJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // Konfiguracja konfiguracji aplikacji z appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Pobierz connection string z konfiguracji
            string connectionString = configuration.GetConnectionString("con");

            // Konfiguracja DbContextu
            var optionsBuilder = new DbContextOptionsBuilder<ClubDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            using (var dbContext = new ClubDbContext(optionsBuilder.Options))
            {
                // Pobierz przeterminowane treningi indywidualne
                var outdatedIndividualTrainings = dbContext.IndividualTraining
                    .Where(it => it.EndTraining < DateTime.Now)
                    .ToList();

                // Usuń przeterminowane treningi indywidualne
                dbContext.IndividualTraining.RemoveRange(outdatedIndividualTrainings);

                // Pobierz przeterminowane treningi grupowe
                var outdatedGroupTrainings = dbContext.GroupTraining
                    .Where(gt => gt.EndTraining < DateTime.Now)
                    .ToList();

                // Usuń przeterminowane treningi grupowe
                dbContext.GroupTraining.RemoveRange(outdatedGroupTrainings);

                // Zapisz zmiany w bazie danych
                await dbContext.SaveChangesAsync();

                Console.WriteLine($"Usunięto {outdatedIndividualTrainings.Count} przeterminowanych treningów indywidualnych.");
                Console.WriteLine($"Usunięto {outdatedGroupTrainings.Count} przeterminowanych treningów grupowych.");
            }
        }
    }

}
