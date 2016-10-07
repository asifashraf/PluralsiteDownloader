using System.Collections.Generic;

namespace AnimalMarketDal.DomainModel
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class AnimalType
    {
        public AnimalType()
        {
            EventDataValues = new List<EventData>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public double MeanCost { get; set; }

        public virtual ICollection<EventData> EventDataValues { get; set; }
    }

    public class EventData
    {
        public long EventDataId { get; set; }
        public int Factor { get; set; }
        public string StringTestId { get; set; }
        public double FixChange { get; set; }

        public long AnimalTypeId { get; set; }
        public virtual AnimalType AnimalType { get; set; }
    }
}
