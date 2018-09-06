using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ContactApi.Data;
using ContactApi.Data.Services;
using ContactApi.Web.Common;
using ContactApi.Web.Common.Logging;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using log4net.Config;
using NHibernate;
using NHibernate.Context;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;

namespace ContactApi.Web.Api
{
    public class NinjectConfigurator
    {
        public void Configure(IKernel container)
        {
            AddBindings(container);
        }

        private void AddBindings(IKernel container)
        {
            ConfigureLog4Net(container);
            ConfigureContactsController(container);
        }

        private void ConfigureLog4Net(IKernel container)
        {
            XmlConfigurator.Configure();

            var logManager = new LogManagerAdapter();
            container.Bind<ILogManager>().ToConstant(logManager);
        }

        private void ConfigureContactsController(IKernel container)
        {
            container.Bind<IContactService>().To<ContactService>().InSingletonScope();
        }
    }
}