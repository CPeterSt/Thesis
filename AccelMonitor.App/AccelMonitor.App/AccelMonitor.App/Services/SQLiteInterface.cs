using SQLite;

namespace AccelMonitor.App.Services
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
