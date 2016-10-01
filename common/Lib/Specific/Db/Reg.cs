using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Specific.Db
{
    public class Reg
    {
        //    public static void RegisterTypes(IUnityContainer container)
        //{
        //    // Add your register logic here...
        //    var myAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("AnimalMarket")).ToArray();
 
        //    container.RegisterType(typeof(Startup));
 
        //    container.RegisterTypes(
        //         UnityHelpers.GetTypesWithCustomAttribute<LifecycleSingletonAttribute>(myAssemblies),
        //         WithMappings.FromMatchingInterface,
        //         WithName.Default,
        //         WithLifetime.ContainerControlled,
        //         null
        //        ).RegisterTypes(
        //                UnityHelpers.GetTypesWithCustomAttribute<LifecycleTransientAttribute>(myAssemblies),
        //                WithMappings.FromMatchingInterface,
        //                WithName.Default,
        //                WithLifetime.Transient);
 
        //    container.RegisterType(typeof(AnimalContext));
        //}


        //    public static IEnumerable<Type> GetTypesWithCustomAttribute<T>(Assembly[] assemblies)
        //    {
        //        foreach (var assembly in assemblies)
        //        {
        //            foreach (Type type in assembly.GetTypes())
        //            {
        //                if (type.GetCustomAttributes(typeof(T), true).Length > 0)
        //                {
        //                    yield return type;
        //                }
        //            }
        //        }
        //    }
    }
}
