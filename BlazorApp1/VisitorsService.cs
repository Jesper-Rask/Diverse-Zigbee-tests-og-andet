namespace BlazorApp1
{
    public class VisitorsService
    {
        private int visitorsCount;
        public VisitorsService()
        {
            visitorsCount = 0;
        }
        public int VisitorsCount()
        {
            return visitorsCount;
        }
        public void AddVisitorsCount()
        {
            visitorsCount++;
        }
    }
}
