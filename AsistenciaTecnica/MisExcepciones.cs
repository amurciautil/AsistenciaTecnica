using System;

namespace AsistenciaTecnica
{ 
    public class MisExcepciones : Exception
    {
        public MisExcepciones(string message) : base(message)
        {
        }
    }
}
