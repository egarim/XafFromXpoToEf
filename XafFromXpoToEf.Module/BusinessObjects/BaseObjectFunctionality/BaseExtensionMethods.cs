﻿using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XafFromXpoToEf.Module.BusinessObjects.BaseObjectFunctionality
{
  
    public static  class BaseExtensionMethods
    {
      
        public static ModelBuilder AddSoftDeleteForChildsOf(this ModelBuilder modelBuilder, Type c)
        {
            IEnumerable<Type> entityTypes = GetChildTypes(c);

            // Get the generic type of configuration class
            var configType = typeof(MyBaseEfObjectConfigurator<>);

            ApplyConfiguration(modelBuilder, entityTypes, configType);
            return modelBuilder;
        }

        private static void ApplyConfiguration(ModelBuilder modelBuilder, IEnumerable<Type> entityTypes, Type configType)
        {
            foreach (var type in entityTypes)
            {

                // Make a specific configuration instance for the entity type
                var specificType = configType.MakeGenericType(type);
                var configurationInstance = Activator.CreateInstance(specificType);

                // Apply this configuration instance
                modelBuilder.ApplyConfiguration((dynamic)configurationInstance);
            }
        }

        public static ModelBuilder AddTimeStampConcurrencyForChildsOf(this ModelBuilder modelBuilder, Type c)
        {
            IEnumerable<Type> entityTypes = GetChildTypes(c);

            // Get the generic type of configuration class
            var configType = typeof(TimeStampConcurrencyConfigurator<>);

            ApplyConfiguration(modelBuilder, entityTypes, configType);
            return modelBuilder;
        }
        public static ModelBuilder WithDefaultStringLenghtForChildsOf(this ModelBuilder modelBuilder, Type c,int DefaultLength)
        {
            IEnumerable<Type> entityTypes = GetChildTypes(c);

            // Get the generic type of configuration class
            var configType = typeof(DefaultStringConfiguration<>);

            foreach (var type in entityTypes)
            {

                // Make a specific configuration instance for the entity type
                var specificType = configType.MakeGenericType(type);
                var configurationInstance = Activator.CreateInstance(specificType, DefaultLength);

                // Apply this configuration instance
                modelBuilder.ApplyConfiguration((dynamic)configurationInstance);
            }

            return modelBuilder;
        }
        private static IEnumerable<Type> GetChildTypes(Type c)
        {
            // Get all entity types derived from 'BaseEntity'
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                {
                    return t.IsSubclassOf(c) && !t.IsAbstract;
                });
        }
    
    
    
    }
}
