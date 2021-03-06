﻿using Autofac;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API
{
    public class APIModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public APIModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthRepository>().As<IAuthRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DatingRepository>().As<IDatingRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LogUserActivity>()
                .AsSelf()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
