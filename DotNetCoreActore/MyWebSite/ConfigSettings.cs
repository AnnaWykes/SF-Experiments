using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebSite
{
    using System.Fabric;
    using System.Fabric.Description;

    public class ConfigSettings
    {
        public ConfigSettings(StatelessServiceContext context)
        {
            context.CodePackageActivationContext.ConfigurationPackageModifiedEvent += this.CodePackageActivationContext_ConfigurationPackageModifiedEvent;
            this.UpdateConfigSettings(context.CodePackageActivationContext.GetConfigurationPackageObject("Config").Settings);
        }

        public string GuestExeBackendServiceName { get; private set; }

        public string StatefulBackendServiceName { get; private set; }

        public string StatelessBackendServiceName { get; private set; }

        public string ActorBackendServiceName { get; private set; }

     //   public string ProducstActorBackendServiceName { get;
     //       set ""; }

        public int ReverseProxyPort { get; private set; }


        private void CodePackageActivationContext_ConfigurationPackageModifiedEvent(object sender, PackageModifiedEventArgs<ConfigurationPackage> e)
        {
            this.UpdateConfigSettings(e.NewPackage.Settings);
        }

        private void UpdateConfigSettings(ConfigurationSettings settings)
        {
            ConfigurationSection section = settings.Sections["MyConfigSection"];
            this.ActorBackendServiceName = section.Parameters["ActorBackendServiceName"].Value;
        }
    }
}
