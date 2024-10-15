using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BoxnMove.Models.Response
{
    [DataContract]
    public class ServiceResponse<T>
    {
        [DataMember]
        public T Data { get; set; }
        [DataMember]
        public int Status { get; set; }

        //[DataMember]
        //public bool IsRetryRequired { get; set; }

        [DataMember]
        public string Message { get; set; }
        //[DataMember]
        //public int ErrorCode { get; set; }
        public long TotalCounts { get; set; }

        //[DataMember]
        //public bool Code { get; set; }
        public long UserStatus { get; set; }

        public ServiceResponse() { }

        public ServiceResponse(ref T data)
        {
            this.Data = data;
        }
    }
}
