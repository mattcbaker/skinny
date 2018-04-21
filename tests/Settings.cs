namespace Skinny
{
    public class Settings
    {
        static Settings()
        {
            DotNetEnv.Env.Load();
        }

        public static string ConnectionString => DotNetEnv.Env.GetString("SKINNY_CONNECTION_STRING");
    }
}