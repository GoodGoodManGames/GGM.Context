using System;
using System.Collections.Generic;
using System.Text;

namespace GGMContext.Context.Exception
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
            : base($"{nameof(CreateManagedError)} : {createManagedError} - 객체 생성에 실패하였습니다.")
        {
            CreateManagedError = createManagedError;
        }

        public CreateManagedError CreateManagedError { get; }
    }
}
