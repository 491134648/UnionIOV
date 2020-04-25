using IotGatewayServer.Services;
using JT808.Protocol;
using Microsoft.Extensions.Logging;
using System;
using Union.Gateway.Abstractions;
using Union.Gateway.MsgLogging;
using Union.Gateway.Traffic;
using Union.Gateway.Transmit;

namespace IotGatewayServer.Impl
{
    public class UnionNormalReplyMessageHandlerImpl : UnionNormalReplyMessageHandler
    {
        private readonly ILogger logger;
        private readonly IUnionTraffic jT808Traffic;
        private readonly IUnionMsgLogging jT808MsgLogging;
        private readonly UnionTransmitService jT808TransmitService;
        private readonly IDeviceService _deviceService;
        public UnionNormalReplyMessageHandlerImpl(
            UnionTransmitService jT808TransmitService,
            IUnionMsgLogging jT808MsgLogging,
            IUnionTraffic jT808Traffic,
            ILoggerFactory  loggerFactory,
            IJT808Config jT808Config,
            IDeviceService deviceService) : base(jT808Config)
        {
            this.jT808TransmitService = jT808TransmitService;
            this.jT808Traffic = jT808Traffic;
            this.jT808MsgLogging = jT808MsgLogging;
            _deviceService = deviceService;
            logger =loggerFactory.CreateLogger("JT808NormalReplyMessageHandlerImpl");
            //添加自定义消息
            HandlerDict.Add(0x9999, Msg0x9999);
        }
        /// <summary>
        /// 重写消息处理器
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        public override byte[] Processor(JT808HeaderPackage request, IUnionSession session)
        {
            //AOP 可以自定义添加一些东西:上下行日志、数据转发
            logger.LogDebug("可以自定义添加一些东西:上下行日志、数据转发");
            //流量
            jT808Traffic.Increment(request.Header.TerminalPhoneNo, DateTime.Now.ToString("yyyyMMdd"), request.OriginalData.Length);
            var parameter = (request.Header.TerminalPhoneNo, request.OriginalData.ToArray());
            //转发数据（可同步也可以使用队列进行异步）
            try
            {
                jT808TransmitService.SendAsync(parameter);
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"");
            }
            //上行日志（可同步也可以使用队列进行异步）
            jT808MsgLogging.Processor(parameter, UnionMsgLoggingType.up);
            //处理上行消息
            var down= base.Processor(request, session);
            //下行日志（可同步也可以使用队列进行异步）
            jT808MsgLogging.Processor((request.Header.TerminalPhoneNo, down), UnionMsgLoggingType.down);
            return down;
        }
        /// <summary>
        /// 重写终端授权信息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public override byte[] Msg0x0102(JT808HeaderPackage request, IUnionSession session)
        {
            logger.LogDebug("重写自带Msg0x0102的消息");
            return base.Msg0x0102(request, session);
        }
        /// <summary>
        /// 重写自带的消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session"></param>
        public override byte[] Msg0x0200(JT808HeaderPackage request, IUnionSession session)
        {
            logger.LogDebug("重写自带Msg0x0200的消息");
            return base.Msg0x0200(request, session);
        }

        /// <summary>
        /// 自定义消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public byte[] Msg0x9999(JT808HeaderPackage request, IUnionSession session)
        {
            logger.LogDebug("自定义消息");
            return default;
        }
    }
}
