using System;
using System.Collections.Generic;
using System.Text;

namespace GGMContext.Context.Factory
{
    /// <summary>
    ///     객체를 만들어 내는 Factory의 인터페이스입니다.
    /// </summary>
    interface IFactory
    {
        object Create(Type type, object[] parameters);
    }
}
