using System;
namespace Invoice.Data.Interfaces
{
    public interface IBaseDto<TKey> where TKey : IComparable
    {
        TKey Id { get; set; }
    }
}
