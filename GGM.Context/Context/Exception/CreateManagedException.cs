namespace GGM.Context.Exception
{
    public enum CreateManagedError
    {
        NotManagedClass,
        UnsupportedManagedType,
        NotExistMatchedConstructor
    }

    public class CreateManagedException : System.Exception
    {
        public CreateManagedException(CreateManagedError createManagedError)
            : base($"Fail to create managed. {nameof(CreateManagedError)} : {createManagedError}")
        {
            CreateManagedError = createManagedError;
        }

        public CreateManagedError CreateManagedError { get; }

        public static void Check(bool condition, CreateManagedError createManagedException)
        {
            if (!condition)
                throw new CreateManagedException(createManagedException);
        }
    }
}