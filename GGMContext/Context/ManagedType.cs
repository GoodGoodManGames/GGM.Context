namespace GGMContext.Context
{
    /// <summary>
    ///     ManagedClass의 생명주기 타입
    /// </summary>
    public enum ManagedType
    {
        /// <summary>
        ///     ManagedPool에서 오직 하나만 존재합니다. 이는 ApplicationContext와 수명을 같이합니다.
        /// </summary>
        Singleton,

        /// <summary>
        ///     주입 시 마다 새로 생성합니다. 이는 해당 주입을 요청한 클래스와 수명을 같이합니다.
        /// </summary>
        Proto
    }
}