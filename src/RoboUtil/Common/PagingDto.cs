using System;
using System.Runtime.Serialization;

namespace RoboUtil.Common
{
    [Serializable]
    [DataContract]
    public class PagingDto
    {
        [DataMember]
        public int pageNumber { get; set; }

        [DataMember]
        public int pageSize { get; set; }

        [DataMember]
        public string orderBy { get; set; }

        [DataMember]
        public string order { get; set; }

        [DataMember]
        public int count { get; set; }
    }
}