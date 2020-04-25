using System;

namespace SuperSocket.ProtoBase
{
    /// <summary>
    /// Default Pipeline Filter Factory
    /// </summary>
    /// <typeparam name="TPackageInfo"></typeparam>
    /// <typeparam name="TPipelineFilter"></typeparam>
    public class DefaultPipelineFilterFactory<TPackageInfo, TPipelineFilter> : PipelineFilterFactoryBase<TPackageInfo>
        where TPackageInfo : class
        where TPipelineFilter : IPipelineFilter<TPackageInfo>, new()
    {
        public DefaultPipelineFilterFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }

        protected override IPipelineFilter<TPackageInfo> CreateCore(object client)
        {
            return new TPipelineFilter();
        }
    }
}