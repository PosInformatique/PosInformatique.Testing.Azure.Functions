//-----------------------------------------------------------------------
// <copyright file="FunctionContextImplementation.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Testing.Azure.Functions.Http
{
    using global::Azure.Core.Serialization;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.DependencyInjection;

    internal sealed class FunctionContextImplementation : FunctionContext
    {
        private readonly HttpRequestDataMock request;

        private IServiceProvider? serviceProvider;

        public FunctionContextImplementation(HttpRequestDataMock request)
        {
            this.request = request;
        }

        public override string InvocationId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string FunctionId
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override TraceContext TraceContext
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override BindingContext BindingContext
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override RetryContext RetryContext
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override IServiceProvider InstanceServices
        {
            get
            {
                if (this.serviceProvider is null)
                {
                    this.ConfigureDefaultServices();

                    this.serviceProvider = this.request.Services.BuildServiceProvider();
                }

                return this.serviceProvider;
            }

            set
            {
                this.serviceProvider = value;
            }
        }

        public override FunctionDefinition FunctionDefinition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override IDictionary<object, object> Items
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override IInvocationFeatures Features
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private void ConfigureDefaultServices()
        {
            this.request.Services.Configure<WorkerOptions>(opt =>
            {
                opt.Serializer = new JsonObjectSerializer();
            });
        }
    }
}
