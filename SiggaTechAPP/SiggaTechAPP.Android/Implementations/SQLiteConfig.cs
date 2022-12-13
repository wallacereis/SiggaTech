using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;
using SiggaTechAPP.Droid.Implementations;
using SiggaTechAPP.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteConfig))]
namespace SiggaTechAPP.Droid.Implementations
{
    public class SQLiteConfig : ISQLiteConfig
    {
        public string _databaseSQLitePath;
        public string DatabaseSQLitePath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_databaseSQLitePath))
                {
                    //------------ Create Local DataBase in SQLite ------------
                    IFolder folder = new LocalRootFolder();
                    _databaseSQLitePath = folder.Path;
                }
                return _databaseSQLitePath;
            }
        }
    }
}