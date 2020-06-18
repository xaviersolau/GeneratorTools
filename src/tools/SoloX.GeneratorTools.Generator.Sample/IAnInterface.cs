using System;
using System.Collections.Generic;
using System.Text;
using SoloX.GeneratorTools.Attributes;

namespace SoloX.GeneratorTools.Generator.Sample
{
    [Factory]
    public interface IAnInterface
    {
        [Dependency]
        IDependency Dependency { get; } // Shall we really use this dependency here ???

        string Property1 { get; } // read only property

        string Property2 { get; set; } // read/write property

        string Property3 { get; }

        IAnOtherInterface Property4 { get; }
    }
}
