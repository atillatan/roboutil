using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using RoboUtil.dto;
using RoboUtil.utils;
using log4net.Repository.Hierarchy;

namespace RoboUtil.dto
{
    [Serializable]
    [DataContract]
    public abstract class BaseDto : ICloneable, IDeserializationCallback, IComparable
    {
        public BaseDto()
        {
            IsActive = true;
        }
        public int Pk { get; set; }
        public DateTime? DtCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? DtUpdated { get; set; }
        public int? UpdatedBy { get; set; }
        public bool? IsActive { get; set; }
        public abstract string ToDescription();
        public void OnDeserialization(Object o)
        {
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public abstract T Copy<T>() where T : class;

        protected T BaseCopy<T>(BaseDto dto) where T : class
        {
            dto.Pk = this.Pk;
            dto.DtCreated = this.DtCreated;
            dto.CreatedBy = this.CreatedBy;
            dto.DtUpdated = this.DtUpdated;
            dto.UpdatedBy = this.UpdatedBy;
            dto.IsActive = this.IsActive;
            return dto as T;
        }
      
        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1;

            BaseDto baseDto = obj as BaseDto;
            if (baseDto != null)
                return this.Pk.CompareTo(baseDto.Pk);
            else
                throw new ArgumentException("Object is not a BaseDto");
        }

        public override bool Equals(Object obj)
        {
            BaseDto baseDto = obj as BaseDto;
            if (baseDto == null)
                return false;
            else
                return Pk.Equals(baseDto.Pk);
        }
        public override int GetHashCode()
        {
            return this.Pk.GetHashCode();
        }

        public  T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(this.GetType());
        }

    }
}
