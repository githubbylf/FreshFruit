using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshFruit.Common.CommonUtils
{
    public struct ErrorCode
    {

        //自定义规则
        //F00x0100
        public int Code;
        public string Msg;

        public ErrorCode(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }

        public ErrorCode SetMsg(string msg)
        {
            Msg = String.Format("{0}｛{1}｝", Msg, msg);
            return this;
        }

        public static readonly ErrorCode SUCCESS = new ErrorCode(0, "SUCCESS");

        #region System Trouble -(0001~1000)

        public static readonly ErrorCode ERROR_SOAP = new ErrorCode(-1, "网络传输错误");

        #endregion

        #region System Member  -(1000~2000)

        #endregion

        #region System Order   -(2000~3000)

        #endregion

        #region System Product -(3000~4000)

        #endregion

        #region System Other   -(4000~5000)

        #endregion
    }
}
