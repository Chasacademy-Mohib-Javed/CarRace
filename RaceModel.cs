using System;
namespace CarRace
{
    public class CarRaceModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Speed { get; set; }
        public int TimeSec { get; set; }
        public double DistanceKm { get; set; }
        public int PenaltyTimer { get; set; }
        public int PreviousChance { get; set; }

        public double RemainingDistance(double racelength)
        {

            return racelength - DistanceKm;
        }

    }
}