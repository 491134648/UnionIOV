using System.IO.Pipelines;
using SuperSocket.ProtoBase;

namespace SuperSocket.Channel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPipeChannel
    {
        Pipe In { get; }

        Pipe Out { get; }

        IPipelineFilter PipelineFilter { get; }
    }
}
