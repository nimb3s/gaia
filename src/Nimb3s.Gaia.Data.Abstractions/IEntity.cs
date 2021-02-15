//TODO: create a nuget package out of this lib
using System;

namespace Nimb3s.Data.Abstractions
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
