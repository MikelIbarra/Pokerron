namespace PokerronBank.Model
{
    public class DataBaseVersion : Entity
    {
        public string Version { get; set; }
       
        public DataBaseVersion()
        {

        }
        public DataBaseVersion(string version)
        {
            Version = version;
        }
    }
}