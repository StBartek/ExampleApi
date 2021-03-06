//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TtsService
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TtsDescription", Namespace="http://schemas.datacontract.org/2004/07/WindEnergy.Common.SirenLibrary.TtsService" +
        "")]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(TtsService.TtsDescriptionWithLng))]
    public partial class TtsDescription : object
    {
        
        private string PathField;
        
        private string TextField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Path
        {
            get
            {
                return this.PathField;
            }
            set
            {
                this.PathField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Text
        {
            get
            {
                return this.TextField;
            }
            set
            {
                this.TextField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.Runtime.Serialization.DataContractAttribute(Name="TtsDescriptionWithLng", Namespace="http://schemas.datacontract.org/2004/07/WindEnergy.Common.SirenLibrary.TtsService" +
        "")]
    public partial class TtsDescriptionWithLng : TtsService.TtsDescription
    {
        
        private string VoiceNameField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VoiceName
        {
            get
            {
                return this.VoiceNameField;
            }
            set
            {
                this.VoiceNameField = value;
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="TtsService.ITtsGenerator")]
    public interface ITtsGenerator
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITtsGenerator/CreateAudioFile", ReplyAction="http://tempuri.org/ITtsGenerator/CreateAudioFileResponse")]
        System.Threading.Tasks.Task CreateAudioFileAsync(TtsService.TtsDescription desc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITtsGenerator/CreateAudioFileSync", ReplyAction="http://tempuri.org/ITtsGenerator/CreateAudioFileSyncResponse")]
        System.Threading.Tasks.Task<bool> CreateAudioFileSyncAsync(TtsService.TtsDescription desc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITtsGenerator/CreateAudioFileInLngSync", ReplyAction="http://tempuri.org/ITtsGenerator/CreateAudioFileInLngSyncResponse")]
        System.Threading.Tasks.Task<bool> CreateAudioFileInLngSyncAsync(TtsService.TtsDescriptionWithLng desc);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITtsGenerator/CreateBatchAudioFile", ReplyAction="http://tempuri.org/ITtsGenerator/CreateBatchAudioFileResponse")]
        System.Threading.Tasks.Task CreateBatchAudioFileAsync(System.Collections.Generic.List<TtsService.TtsDescription> descList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITtsGenerator/CreateBatchAudioFileSync", ReplyAction="http://tempuri.org/ITtsGenerator/CreateBatchAudioFileSyncResponse")]
        System.Threading.Tasks.Task<bool> CreateBatchAudioFileSyncAsync(System.Collections.Generic.List<TtsService.TtsDescription> descList);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ITtsGenerator/GetOutboundLicenceCount", ReplyAction="http://tempuri.org/ITtsGenerator/GetOutboundLicenceCountResponse")]
        System.Threading.Tasks.Task<int> GetOutboundLicenceCountAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public interface ITtsGeneratorChannel : TtsService.ITtsGenerator, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.2")]
    public partial class TtsGeneratorClient : System.ServiceModel.ClientBase<TtsService.ITtsGenerator>, TtsService.ITtsGenerator
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public TtsGeneratorClient() : 
                base(TtsGeneratorClient.GetDefaultBinding(), TtsGeneratorClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_ITtsGenerator.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public TtsGeneratorClient(EndpointConfiguration endpointConfiguration) : 
                base(TtsGeneratorClient.GetBindingForEndpoint(endpointConfiguration), TtsGeneratorClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public TtsGeneratorClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(TtsGeneratorClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public TtsGeneratorClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(TtsGeneratorClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public TtsGeneratorClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task CreateAudioFileAsync(TtsService.TtsDescription desc)
        {
            return base.Channel.CreateAudioFileAsync(desc);
        }
        
        public System.Threading.Tasks.Task<bool> CreateAudioFileSyncAsync(TtsService.TtsDescription desc)
        {
            return base.Channel.CreateAudioFileSyncAsync(desc);
        }
        
        public System.Threading.Tasks.Task<bool> CreateAudioFileInLngSyncAsync(TtsService.TtsDescriptionWithLng desc)
        {
            return base.Channel.CreateAudioFileInLngSyncAsync(desc);
        }
        
        public System.Threading.Tasks.Task CreateBatchAudioFileAsync(System.Collections.Generic.List<TtsService.TtsDescription> descList)
        {
            return base.Channel.CreateBatchAudioFileAsync(descList);
        }
        
        public System.Threading.Tasks.Task<bool> CreateBatchAudioFileSyncAsync(System.Collections.Generic.List<TtsService.TtsDescription> descList)
        {
            return base.Channel.CreateBatchAudioFileSyncAsync(descList);
        }
        
        public System.Threading.Tasks.Task<int> GetOutboundLicenceCountAsync()
        {
            return base.Channel.GetOutboundLicenceCountAsync();
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ITtsGenerator))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_ITtsGenerator))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:8731/Wind.TtsService/TtsGenerator/");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return TtsGeneratorClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_ITtsGenerator);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return TtsGeneratorClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_ITtsGenerator);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_ITtsGenerator,
        }
    }
}
