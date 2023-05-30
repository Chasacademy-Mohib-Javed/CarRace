namespace CarRace
{
    public class CarRace
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any key to start");
            Console.ReadKey();
            CarRaceModel car1 = new CarRaceModel()
            {
                Id = 1,
                Name = "skoda octavia",
                Speed = 100,
                TimeSec = 0,
                DistanceKm = 0,
                PenaltyTimer = 0,
                PreviousChance = 0
            };
            CarRaceModel car2 = new CarRaceModel()
            {
                Id = 2,
                Name = "BMW M4",
                Speed = 100,
                TimeSec = 0,
                DistanceKm = 0,
                PenaltyTimer = 0,
                PreviousChance = 0
            };
            List<CarRaceModel> carList = new()
            {
                car1, car2
            };
            bool first = false;
            int racelength = 25;
            var task1 = RunRace(car1, racelength);
            var task2 = RunRace(car2, racelength);
            var carStatusTask = RaceStatus(carList, racelength);
            var raceTasks = new List<Task> { task1, task2, carStatusTask };

            while (raceTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(raceTasks);
                if (finishedTask == carStatusTask)
                {
                    Console.WriteLine("Race Done");
                }
                else if (finishedTask == task1)
                {

                    if (first == false)
                    {
                        first = true;
                        Console.WriteLine($"{car1.Name} has crossed the finished line first");
                    }
                    else
                    {
                        Console.WriteLine($"{car1.Name} has crossed the finished line");
                    }
                }
                else if (finishedTask == task2)
                {
                    if (first == false)
                    {
                        first = true;
                        Console.WriteLine($"{car2.Name} has crossed the finished line first");
                    }
                    else
                    {
                        Console.WriteLine($"{car2.Name} has crossed the finished line");
                    }
                }
                await finishedTask;
                raceTasks.Remove(finishedTask);
            }
            Console.WriteLine("Main method end");
        }
        public static async Task<CarRaceModel> RunRace(CarRaceModel car, int racelength)
        {
            Console.WriteLine($"{car.Name} starts the race");
            Random random = new();
            int ticTracker = 0;
            while (car.DistanceKm < racelength)
            {
                await Tic();
                ticTracker += 1;
                car.TimeSec += 1;
                if (ticTracker % 30 == 0)
                {
                    int chance = random.Next(1, 51);


                    if (chance == 1)
                    {
                        if (car.PreviousChance == chance)
                        {
                            Console.WriteLine($"{car.Name}'s pit crew dont seem to know what they are doing and lose another 30 sec");
                        }
                        else
                        {
                            Console.WriteLine($"{car.Name} has run out of gas. Refuling for 30 sec");
                        }
                        car.PenaltyTimer += 30;
                    }
                    else if (chance >= 2 && chance <= 3)
                    {
                        if (car.PreviousChance >= 2 && car.PreviousChance <= 3)
                        {
                            Console.WriteLine($"{car.Name}'s crew dont know wich way to turn the bolts and lose another 20 sec");
                        }
                        else
                        {
                            Console.WriteLine($"{car.Name} is changing tiers for 15 sec");
                        }
                        car.PenaltyTimer += 20;
                    }
                    else if (chance >= 4 && chance <= 8)
                    {
                        if (car.PreviousChance >= 4 && car.PreviousChance <= 8)
                        {
                            Console.WriteLine($"{car.Name}'s driver is still trying to get the bug out and loses another 10 sec");
                        }
                        else
                        {
                            Console.WriteLine($"{car.Name} lost 15 sec trying to get a bug out of the car");
                        }
                        car.PenaltyTimer += 10;
                    }
                    else if (chance >= 41 && chance <= 50)
                    {
                        if (car.PreviousChance >= 41 && car.PreviousChance <= 50)
                        {
                            Console.WriteLine($"{car.Name}'s driver is still sending it a litle to hard & lost 1kmh speed");
                        }
                        else
                        {
                            Console.WriteLine($"{car.Name}'s driver sent the car a little to hard & lost 1kmh speed");
                        }
                        car.Speed -= 1;
                    }
                    car.PreviousChance = chance;

                }


                if (car.PenaltyTimer > 0)
                {
                    car.PenaltyTimer += -1;
                }
                else
                {
                    car.DistanceKm += car.Speed / Math.Pow(60, 2);
                }

            }

            return car;
        }
        public async static Task Tic(double delay = 1)
        {
            await Task.Delay(TimeSpan.FromSeconds(delay / 10));
        }

        public async static Task RaceStatus(List<CarRaceModel> cars, int racelength)
        {
            await Task.Delay(1);

            cars.ForEach(Car =>
            {
                Console.WriteLine($"{Car.Name}: {Car.DistanceKm}Km {Car.TimeSec}Sec {Car.Speed}Km/h");
            });

            while (true)
            {


                bool gotkey = false;
                DateTime start = DateTime.Now;

                while ((DateTime.Now - start).TotalSeconds < 1)
                {
                    if (Console.KeyAvailable)
                    {
                        gotkey = true;
                        break;
                    }
                }
                if (gotkey == true)
                {
                    var s = Console.ReadKey();
                    cars.ForEach(Car =>
                    {
                        Console.WriteLine($"{Car.Name}: {Car.DistanceKm}Km {Car.TimeSec}Sec {Car.Speed}Km/h");
                    });
                    gotkey = false;
                }

                var finishedRace = cars.Select(car => car.RemainingDistance(racelength)).Sum();
                if (finishedRace <= 0)
                {
                    cars.ForEach(Car =>
                    {
                        Console.WriteLine($"{Car.Name}: {racelength}Km & Finished at {Car.TimeSec}Sec {Car.Speed}Km/h");
                    });
                    return;
                }
            }
        }

    }
}