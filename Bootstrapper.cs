using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Web.Http;
using Microsoft.Practices.ServiceLocation;

namespace Combo
{
  public static class Bootstrapper
  {
    public static IUnityContainer Initialise()
    {
      var container = BuildUnityContainer();
      //
      var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
      
      section.Configure(container, "containerOne");
      /*var serviceProvider = new UnityServiceLocator(container);
      ServiceLocator.SetLocatorProvider(() => serviceProvider);*/
     //

      DependencyResolver.SetResolver(new UnityDependencyResolver(container));

      
      return container;
    }

    private static IUnityContainer BuildUnityContainer()
    {
      var container = new UnityContainer();

      // register all your components with the container here
      // it is NOT necessary to register your controllers

      // e.g. container.RegisterType<ITestService, TestService>();    
      RegisterTypes(container);

      return container;
    }

    public static void RegisterTypes(IUnityContainer container)
    {
    
    }
  }
}