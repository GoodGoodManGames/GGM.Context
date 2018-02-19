namespace GGM.Context.Attribute
{
    /// <summary>
    /// 클래스가 Configuration 클래스임을 지정하는 Attribute입니다. Singleton타입으로 고정됩니다.
    /// </summary>
    public class ConfigurationAttribute : ManagedAttribute
    {
        public ConfigurationAttribute() : base(ManagedType.Singleton) { }
    }
}