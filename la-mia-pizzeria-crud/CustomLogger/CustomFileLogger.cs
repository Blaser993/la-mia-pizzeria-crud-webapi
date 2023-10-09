namespace la_mia_pizzeria_static.CustomLogger
{
    public class CustomFileLogger :ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.AppendAllText($"C:\\Users\\Utente\\source\\repos\\la-mia-pizzeria-crud\\la-mia-pizzeria-crud\\my-log.txt", $"{DateTime.Now.ToString("yyyy MM dd HH mm ss")}LOG: {message} \n");
        }
    }
}
