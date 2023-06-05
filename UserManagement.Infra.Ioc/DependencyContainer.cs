using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using UserManagement.Domain.Interfaces;
using System.Text;
using UserManagement.Domain.Models;
using UserManagement.Domain.Queries;
using UserManagement.Domain.Commands;
using UserManagement.Domain.Handlers;
using UserManagement.Data.Repositories;

namespace UserManagement.Infra.Ioc
{
    public class DependencyContainer
    {
        public static void RegisterService(IServiceCollection services)
        {
            //Repository
            services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();
            services.AddTransient<IGenericRepository<Role>, GenericRepository<Role>>();
            services.AddTransient<IGenericRepository<ChefCentre>, GenericRepository<ChefCentre>>();




            services.AddTransient<IRequestHandler<GetListGenericQuery<User>, IEnumerable<User>>, GetListGenericHandler<User>>();
            services.AddTransient<IRequestHandler<GetGenericQuery<User>, User>, GetGenericHandler<User>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<User>, User>, PutGenericHandler<User>>();
            services.AddTransient<IRequestHandler<RemoveGenericCommand<User>, User>, RemoveGenericHandler<User>>();
            services.AddTransient<IRequestHandler<AddGenericCommand<User>, User>, AddGenericHandler<User>>();




            services.AddTransient<IRequestHandler<GetListGenericQuery<Role>, IEnumerable<Role>>, GetListGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<GetGenericQuery<Role>, Role>, GetGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<Role>, Role>, PutGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<RemoveGenericCommand<Role>, Role>, RemoveGenericHandler<Role>>();
            services.AddTransient<IRequestHandler<AddGenericCommand<Role>, Role>, AddGenericHandler<Role>>();




            services.AddTransient<IRequestHandler<GetListGenericQuery<ChefCentre>, IEnumerable<ChefCentre>>, GetListGenericHandler<ChefCentre>>();
            services.AddTransient<IRequestHandler<GetGenericQuery<ChefCentre>, ChefCentre>, GetGenericHandler<ChefCentre>>();
            services.AddTransient<IRequestHandler<PutGenericCommand<ChefCentre>, ChefCentre>, PutGenericHandler<ChefCentre>>();
            services.AddTransient<IRequestHandler<RemoveGenericCommand<ChefCentre>, ChefCentre>, RemoveGenericHandler<ChefCentre>>();
            services.AddTransient<IRequestHandler<AddGenericCommand<ChefCentre>, ChefCentre>, AddGenericHandler<ChefCentre>>();

        }
    }
}
