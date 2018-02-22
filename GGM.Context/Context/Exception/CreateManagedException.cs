namespace GGM.Context.Exception
{
    /// <summary>
    /// CreateManagedException이 발생하는 에러의 타입입니다.
    /// </summary>
    public enum CreateManagedError
    {
        /// <summary>
        /// 해당 클래스는 Managed 클래스가 아님.
        /// </summary>
        NotManagedClass,
        /// <summary>
        /// 지원되지 않는 Managed 타입
        /// </summary>
        UnsupportedManagedType,
        /// <summary>
        /// 생성자가 매칭되는 것이 없습니다.
        /// </summary>
        NotExistMatchedConstructor
    }

    //TODO: 추후 생성과 등록을 분리
    /// <summary>
    /// Managed객체를 생성하거나 등록할때 발생하는 예외입니다.
    /// </summary>
    public class CreateManagedException : System.Exception
    {
        public CreateManagedException(CreateManagedError createManagedError)
            : base($"Fail to create managed. {nameof(CreateManagedError)} : {createManagedError}")
        {
            CreateManagedError = createManagedError;
        }

        public CreateManagedError CreateManagedError { get; }

        /// <summary>
        /// 조건을 체크합니다.
        /// </summary>
        /// <param name="condition">항상 참이어야 하는 조건</param>
        /// <param name="createManagedException">조건을 불만족 했을 시의 에러타입</param>
        /// <exception cref="CreateManagedException">Managed객체 생성, 등록 예외</exception>
        public static void Check(bool condition, CreateManagedError createManagedException)
        {
            if (!condition)
                throw new CreateManagedException(createManagedException);
        }
    }
}