using Castle.DynamicProxy;
using FreshFruit.Common.BaseCommon;
using FreshFruit.Common.BaseCommon.Extended;
using FreshFruit.Common.Components.Logs;
using FreshFruit.Common.Components.MsgCode;
using System;
using System.Text;

namespace FreshFruit.IoC.DI
{
    public class LoggingInterceptor: IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (BusinessException ex)
            {
                StringBuilder parms = new StringBuilder(JsonHelper.SerializeObject(invocation.Arguments));
                if (string.IsNullOrWhiteSpace(parms.ToString()))
                {
                    foreach (var a in invocation.Arguments)
                    {
                        parms.Append(string.Format("{0}&", JsonHelper.SerializeObject(a)));
                    }
                }

                //只有业务层仅返回true/false的时候，才会抛出该异常（服务层响应BaseResponse类型）
                var message = string.Format("Message:{0} /n/r Arguments:{1} "
                    , ex.Message, parms.ToString());

                NLogHelper.Error(string.Format("Method:{0} /n/r  Message:{1}  /n/r Arguments:{2}  "
                    , ex.ToString()
                    , invocation.Method.ToString()
                    , MessageCodesConfig.GetMessageContent(message)));
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex.ToString(), invocation.Method.ToString()
                    , string.Format("Message:{0} /n/r Arguments:{1} "
                    , ex.Message, JsonHelper.SerializeObject(invocation.Arguments)));

            }
        }
    }
}
