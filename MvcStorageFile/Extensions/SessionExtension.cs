using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcStorage.Extensions
{
    public static class SessionExtension
    {
        public static void SetObject<T>(this ISession session
            , string key, object value)
        {
            //ALMACENAMOS EL OBJETO EN LA SESSION COMO STRING
            //PERO CREADO DE FORMA SERIALIZADA CON JSON
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            //RECUPERAMOS EL STRING JSON QUE REPRESENTA A UN OBJETO
            String jsonstring = session.GetString(key);
            if (jsonstring == null)
            {
                //DEVOLVEMOS EL VALOR POR DEFECTO DEL OBJETO QUE NOS
                //HAN ENVIADO, QUE SERA UN null
                return default(T);
            }
            //DEVOLVEMOS EL OBJETO SERIALIZADO
            return JsonConvert.DeserializeObject<T>(jsonstring);
        }
    }
}
