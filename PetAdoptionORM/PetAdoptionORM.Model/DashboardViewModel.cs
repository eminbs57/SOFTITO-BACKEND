using System.Collections.Generic;

namespace PetAdoptionORM.Model
{
    public class DashboardViewModel
    {
        public int TotalSpecies { get; set; }
        public int TotalBreeds { get; set; }
        public int TotalPets { get; set; }
        public int TotalReports { get; set; }

        // Data for Chart.js
        public List<string> ChartLabels { get; set; } = new List<string>();
        public List<int> ChartData { get; set; } = new List<int>();

        // Recent Activities
        public List<ActivityItem> RecentActivities { get; set; } = new List<ActivityItem>();
        public List<string> SystemLogs { get; set; } = new List<string>();
    }

    public class ActivityItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public string IconColorClass { get; set; }
        public string TimeAgo { get; set; }
    }
}
