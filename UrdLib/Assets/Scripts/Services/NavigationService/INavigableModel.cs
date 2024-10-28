using System;

namespace Urd.Navigation
{
    public interface INavigableModel
    {
        public int Id { get; }
        public Enum Type { get; }
    }
}