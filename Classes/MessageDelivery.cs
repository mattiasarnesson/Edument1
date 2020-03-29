using System;

namespace Edument1.Classes
{
    public class MessageDelivery
    {

        private String code;
        private String message;

        public MessageDelivery(string code, string message)
        {
            this.Code = code;
            this.Message = message;
        }

        public string Code { get => code; set => code = value; }
        public string Message { get => message; set => message = value; }
    }
}