using System;
using MediatR;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Commands
{
    public class PutGenericCommand<TEntity> : IRequest<TEntity> where TEntity : class
    {
        public PutGenericCommand(TEntity entity)
        {
            Entity = entity;
        }
        public TEntity Entity { get; }
    }
}
