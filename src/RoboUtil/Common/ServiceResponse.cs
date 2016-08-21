using System.Runtime.Serialization;

namespace RoboUtil.Common
{
    [DataContract]
    public class ServiceResponse<T> 
    {

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public ResultType ResultType { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public T Data { get; set; }

        private ServiceResponse()
        {
        }

        public ServiceResponse(bool IsSuccess, ResultType ResultType, string Message)
            : this(IsSuccess, ResultType, Message, default(T), 0)
        {
        }

        public ServiceResponse(bool IsSuccess, ResultType ResultType, string Message, T Data)
            : this(IsSuccess, ResultType, Message, Data, 0)
        {
        }

        public ServiceResponse(bool IsSuccess, ResultType ResultType, string Message, T Data, int TotalCount)
        {
            this.IsSuccess = IsSuccess;
            this.ResultType = ResultType;
            this.Message = Message;
            this.Data = Data;
            this.TotalCount = TotalCount;
        }
    }

    public enum ResultType
    {
        Information = 1,
        Validation = 2,
        Warning = 4,
        Error = 5
    };
}
