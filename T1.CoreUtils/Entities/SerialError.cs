using System;
using System.Runtime.Serialization;

namespace T1.CoreUtils.Entities
{
    [DataContract]
    public class SerialError
    {
        [DataMember]
        public string ExceptionType { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string StackTrace { get; set; }

        [DataMember]
        public SerialError InnerException { get; set; }

        public SerialError(Exception ex)
        {
            if (ex == null)
            {
                return;
            }
            this.ExceptionType = ex.GetType().Name;
            this.Message = ex.Message;

            if (App.UseDebugMode)
            {
                this.StackTrace = ex.StackTrace;
            }

            if (ex.InnerException != null)
            {
                this.InnerException = new SerialError(ex.InnerException);
            }
        } //end constructor

        public static SerialError FromObject(Object err)
        {
            Exception ex = err as Exception;
            return (ex == null) ? null : new SerialError(ex);
        }
    }
}
