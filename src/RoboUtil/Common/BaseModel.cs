using System;
using System.Runtime.Serialization;

namespace Core.Common.Model
{
    [Serializable]
    public abstract class BaseModel :   IComparable
    {
        public BaseModel()
        {
            IsActive = true;
        }
       
        public int Id { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public int CreatedBy { get; set; }
        
        public DateTime? UpdateDate { get; set; }
       
        public int? UpdatedBy { get; set; }
        
        public bool IsActive { get; set; }

        public void OnDeserialization(Object o) { }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public virtual int CompareTo(object obj)
        {
            if (obj == null) return 1;

            BaseModel baseModel = obj as BaseModel;
            if (baseModel != null)
                return this.Id.CompareTo(baseModel.Id);
            else
                throw new ArgumentException("Object is not a BaseModel");
        }
        public override bool Equals(Object obj)
        {
            BaseModel baseModel = obj as BaseModel;
            if (baseModel == null)
                return false;
            else
                return Id.Equals(baseModel.Id);
        }
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public T CreateInstance<T>()
        {
            return (T)Activator.CreateInstance(this.GetType());
        }

    }

}
