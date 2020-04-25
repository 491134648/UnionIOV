using System.Collections.Generic;
using Union.Gateway.Abstractions;

namespace IotGatewayServer.Services
{
    /// <summary>
    /// 设备服务
    /// </summary>
    public interface IDeviceService
    {
        /// <summary>
        /// 设备授权校验
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <param name="Extras"></param>
        /// <returns></returns>
        OperateResult Validation(string terminalPhoneNo, Dictionary<string, object> Extras=null);
        /// <summary>
        /// 设备注册校验
        /// </summary>
        /// <param name="terminalPhoneNo"></param>
        /// <param name="isUpdateRegisterInfo">是否更新设备信息</param>
        /// <param name="Extras"></param>
        /// <returns></returns>
        OperateResult RegionValidation(string terminalPhoneNo,bool isUpdateRegisterInfo=true, Dictionary<string, object> Extras=null);
    }
    
}
