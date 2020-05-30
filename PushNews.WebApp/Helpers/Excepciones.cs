using System;
using System.Data.SqlClient;

namespace PushNews.WebApp.Helpers
{
    public static class Excepciones
    {
        public static bool ComprobarViolacionPK(Exception e)
        {
            Exception inner = e;
            while (inner.InnerException != null)
            {
                inner = inner.InnerException;
            }
            return inner is SqlException && ((SqlException)inner).Number == 2627;
        }
    }
}