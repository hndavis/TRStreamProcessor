﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFTestClient.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IStreamOutService")]
    public interface IStreamOutService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreamOutService/GetMessageQueueName", ReplyAction="http://tempuri.org/IStreamOutService/GetMessageQueueNameResponse")]
        string GetMessageQueueName();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IStreamOutService/GetMessageQueueName", ReplyAction="http://tempuri.org/IStreamOutService/GetMessageQueueNameResponse")]
        System.Threading.Tasks.Task<string> GetMessageQueueNameAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IStreamOutServiceChannel : WCFTestClient.ServiceReference1.IStreamOutService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class StreamOutServiceClient : System.ServiceModel.ClientBase<WCFTestClient.ServiceReference1.IStreamOutService>, WCFTestClient.ServiceReference1.IStreamOutService {
        
        public StreamOutServiceClient() {
        }
        
        public StreamOutServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public StreamOutServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreamOutServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public StreamOutServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetMessageQueueName() {
            return base.Channel.GetMessageQueueName();
        }
        
        public System.Threading.Tasks.Task<string> GetMessageQueueNameAsync() {
            return base.Channel.GetMessageQueueNameAsync();
        }
    }
}
