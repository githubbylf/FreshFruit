
namespace FreshFruit.Common.CommonUtils
{

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum ORDER_STATUS
    {
        /// <summary>
        /// 未确认
        /// </summary>
        NEW_STATUS = 0,

        /// <summary>
        /// 已确认
        /// </summary>
        CONFIRM_STATUS = 1,

        /// <summary>
        ///已取消
        /// </summary>
        CANCEL_STATUS = 2,

        /// <summary>
        /// 无效
        /// </summary>
        INVALID_STATUS = 3,

        /// <summary>
        /// 退货
        /// </summary>
        RETURNED_STATUS = 4,

        /// <summary>
        /// 预售
        /// </summary>
        BOOK_STATUS = 5,

        /// <summary>
        /// 部分退货
        /// </summary>
        RETURNED_SOMERETURN = 6,

        /// <summary>
        /// 预付
        /// </summary>
        PRE_STATUS = 7
    }


    /// <summary>
    /// 支付状态
    /// </summary>
    public enum ORDER_PAY_STATUS
    {
        /// <summary>
        /// 未付款
        /// </summary>
        NOT_PAY = 0,

        /// <summary>
        /// 付款中
        /// </summary>
        PAYING = 1,

        /// <summary>
        /// 已付款
        /// </summary>
        PAYED = 2,

        /// <summary>
        /// 付款错误
        /// </summary>
        PAY_ERR = 3,

        /// <summary>
        /// 部分付款
        /// </summary>
        PAY_PART = 4
    }


    /// <summary>
    /// 发货状态
    /// </summary>
    public enum SHIPPING_STATUS
    {
        /// <summary>
        /// 未发货
        /// </summary>
        SHIPPING_NOT_SEND = 0,

        /// <summary>
        /// 备货中
        /// </summary>
        SHIPPING_PREPARE_GOODS = 1,

        /// <summary>
        /// 已出库
        /// </summary>
        SHIPPING_OUT = 2,

        /// <summary>
        /// 已发货
        /// </summary>
        SHIPPING_SEND = 3,

        /// <summary>
        /// 已收货
        /// </summary>
        SHIPPING_RECEIVED = 4

    }

}
