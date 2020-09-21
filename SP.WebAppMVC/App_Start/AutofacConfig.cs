using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using SP.DataAccess.Services;
using SP.DataAccess.Services.Interfaces;

namespace SP.WebAppMVC
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<StudentsService>().As<IStudentsService>();
            //builder.RegisterAssemblyModules(AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("SP")).ToArray());

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}