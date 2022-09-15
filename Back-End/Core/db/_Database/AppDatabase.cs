using Core.Component.Library.SQL;

namespace Services.Database
{
    public class AppDatabase : IDatabase
    {
        public AppDatabase(string cnn)
        {
            this.ConnectionString = cnn;
        }
    }
}