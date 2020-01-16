using System;

namespace Travelo.Core.Domain
{
    public class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity()
        {
        }
    }
}