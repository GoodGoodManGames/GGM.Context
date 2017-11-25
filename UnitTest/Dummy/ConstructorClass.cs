using GGMContext.Context.Attribute;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest.Dummy
{
    public class ConstructorClass
    {
        [AutoWired]
        public ConstructorClass(int first, string second)
        {

        }
    }
}
