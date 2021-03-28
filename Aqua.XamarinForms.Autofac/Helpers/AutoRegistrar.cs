using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Aqua.Core.Attributes;
using Aqua.Core.Extensions;
using Aqua.Core.Helpers;
using Aqua.Core.Interfaces;
using Aqua.XamarinForms.Services.Navigation;

using Autofac;

namespace Aqua.XamarinForms.Autofac.Helpers
{
    internal static class AutoRegistrar
    {
        internal static void RegistrationServices(ContainerBuilder containerBuilder, bool useAutoRegistrationServices, Assembly[] assembliesForSearch)
        {
            if (!useAutoRegistrationServices)
                return;
            
            var assemblies =
                (assembliesForSearch ?? AssemblyHelper.GetDependentAssemblies(typeof(AquaApplication).Assembly))
                    .Union(new[] { typeof(IResolvable).Assembly, typeof(NavigationService).Assembly, typeof(AquaApplication).Assembly })
                    .ToArray();

            var allResolvable = assemblies
                .SelectMany(it => it.GetTypes()
                    .Where(type => typeof(IResolvable).IsAssignableFrom(type) 
                                   && !type.IsOneOf(typeof(IResolvable), typeof(IResolvable<>), typeof(IResolvableFactory<>), typeof(IResolvableFactory<,>), typeof(IResolvableFactory<,,>))
                                   && !type.IsDefined(typeof(ManualRegistrationAttribute))))
                .ToArray();

            var implementations = allResolvable.Where(it => it.IsClass).ToArray();
            var services = allResolvable.Where(it => it.IsInterface).ToArray();

            foreach (var service in services)
            {
                var descendants = GetDescendantLeaves(service, implementations);

                if (!descendants.Any())
                    continue;

                descendants
                    .OrderByDescending(it => (it.GetCustomAttribute(typeof(OrderAttribute), false) as OrderAttribute)?.Value ?? 0)
                    .ForEach(it => RegisterType(containerBuilder, it, service));
            }
            
            implementations
                .Where(it => !it.IsAbstract && !services.Any(s => s.IsAssignableFrom(it)))
                .ForEach(it => RegisterType(containerBuilder, it));
        }

        private static HashSet<Type> GetDescendantLeaves(Type baseType, Type[] typesForSearch)
        {
            var result = new HashSet<Type>();

            typesForSearch
                .Where(it => baseType.IsInterface 
                    ? it.BaseType == typeof(object) && baseType.IsAssignableFrom(it)
                    : it.BaseType == baseType)
                .ForEach(it =>
                {
                    var descendantLeaves = GetDescendantLeaves(it, typesForSearch);
                    result.UnionWith(descendantLeaves.Any() ? descendantLeaves : new HashSet<Type> { it });
                });

            return result.Where(it => !it.IsAbstract).ToHashSet();
        }
        
        private static void RegisterType(ContainerBuilder containerBuilder, Type implementation, Type service = null)
        {
            var registrationBuilder = containerBuilder.RegisterType(implementation).As(service ?? implementation);
            
            if (implementation.IsDefined(typeof(AsSingleInstanceAttribute)) 
                || (service?.IsDefined(typeof(AsSingleInstanceAttribute)) ?? false))
            {
                registrationBuilder.SingleInstance();
            }
        }
    }
}