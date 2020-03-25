﻿using Autofac;
using JetBrains.Annotations;
using Lykke.Service.Dictionaries.Domain.Services;
using Lykke.Service.Dictionaries.DomainServices;
using Lykke.Service.Dictionaries.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Dictionaries.Modules
{
    [UsedImplicitly]
    public class AutofacModule : Module
    {
        private readonly AppSettings _settings;

        public AutofacModule(IReloadingManager<AppSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(
                new MsSqlRepositories.AutofacModule(_settings.DictionariesService.Db.DataConnectionString));

            builder.RegisterType<SalesforceService>()
                .As<ISalesforceService>()
                .SingleInstance();
            builder.RegisterType<CommonInformationService>()
                .As<ICommonInformationService>()
                .SingleInstance()
                .WithParameter("marketingWebsiteUrl", _settings.DictionariesService.Common.CustomerMarketingWebsiteUrl)
                .WithParameter("supportEmail", _settings.DictionariesService.Common.CustomerSupportEmail)
                .WithParameter("supportPhoneNumber", _settings.DictionariesService.Common.CustomerSupportPhoneNumber);
        }
    }
}
