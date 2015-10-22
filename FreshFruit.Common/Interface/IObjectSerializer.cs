namespace FreshFruit.Common.Interface
{
    /// <summary>
	/// 提供序列与反序列化对象的接口。
	/// </summary>
	public interface IObjectSerializer
    {
        /// <summary>
        /// 将一个给定对象序列化成字节流。
        /// </summary>
        /// <typeparam name="TObject">要序列化的对象</typeparam>
        /// <param name="obj">对象型参</param>
        /// <returns></returns>
        byte[] Serialize<TObject>(TObject obj);

        /// <summary>
        /// 将字节流反序列成一个指定类型的对象。
        /// </summary>
        /// <typeparam name="TObject">对象型参</typeparam>
        /// <param name="stream">可反序列化成对象的字节流</param>
        /// <returns></returns>
        TObject Deserialize<TObject>(byte[] stream);
    }
}
