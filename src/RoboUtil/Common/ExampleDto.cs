using System;
using System.Runtime.Serialization;


namespace RoboUtil.Common
{
    public class ExampleDto : BaseDto
    {
        [DataMember]
        public string StringVar { get; set; }

        [DataMember]
        public int IntVar { get; set; }

        [DataMember]
        public DateTime DateTimeVar { get; set; }

        public override string ToString()
        {
            return this.StringVar + " " + IntVar;
        }
    }
}
