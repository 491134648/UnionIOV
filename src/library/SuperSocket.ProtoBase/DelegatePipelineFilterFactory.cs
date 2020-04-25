using System;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// Delegate Pipeline Filter Factory
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    public class DelegatePipelineFilterFactory<TPackageInfo> : PipelineFilterFactoryBase<TPackageInfo>
        where TPackageInfo : class
    {
        private readonly Func<object, IPipelineFilter<TPackageInfo>> _factory;

        public DelegatePipelineFilterFactory(IServiceProvider serviceProvider, Func<object, IPipelineFilter<TPackageInfo>> factory)
            : base(serviceProvider)
        {
            _factory = factory;
        }

        protected override IPipelineFilter<TPackageInfo> CreateCore(object client)
        {
            return _factory(client);
        }
    }
}