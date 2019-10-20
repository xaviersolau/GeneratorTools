# GeneratorTools
[![CircleCI](https://circleci.com/gh/xaviersolau/GeneratorTools.svg?style=svg)](https://circleci.com/gh/xaviersolau/GeneratorTools)
[![Coverage Status](https://coveralls.io/repos/github/xaviersolau/GeneratorTools/badge.svg?branch=master)](https://coveralls.io/github/xaviersolau/GeneratorTools?branch=master)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.GeneratorTools.Core.CSharp.svg)](https://www.nuget.org/packages/SoloX.GeneratorTools.Core.CSharp)

GeneratorTools is a project that helps you to automate C# code generation.
It is written in C# and thanks to .Net Standard, it is cross platform.

It also provides you with an API to parse your existing C# sources (based on Roslyn).

Don't hesitate to post issue, pull request on the project or to fork and improve the project.

## License and credits

GeneratorTools project is written by Xavier Solau. It's licensed under the MIT license.

 * * *

## Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.GeneratorTools.Core.CSharp -version 1.0.0-alpha.17
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.GeneratorTools.Core.CSharp --version 1.0.0-alpha.17
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.GeneratorTools.Core.CSharp" Version="1.0.0-alpha.17" />
```

## How to use it

Note that you can find code examples in this repository in this location: `src/examples`.

### The use case

How many time are you writing a source code that is matching a single pattern?
A code that doesn't bring a lot of value but a code that you need to write and to maintain?

Well, may be this project can help you!

Let's take a simple example were you would like to write a set of classes implementing a given set of interfaces.
The interfaces are all based on a `IModelBase` interface defined as follow.

```csharp
    /// <summary>
    /// Model base interface that defines a IsDirty property.
    /// </summary>
    public interface IModelBase
    {
        /// <summary>
        /// Gets a value indicating whether the model is dirty or not.
        /// The model is going to be dirty as soon as one of its property is set.
        /// </summary>
        bool IsDirty { get; }
    }
```

The model base interface defines a `IsDirty` property that is going to be set once a model implementation property is set.

All you need to do in order to define a model is to write your model interface: `IMyModel` with the properties you want:

```csharp
    /// <summary>
    /// A model definition example with two properties MyFirstProperty and MySecondProperty.
    /// </summary>
    /// <remarks>Setting on of the two properties will set the IsDirty flag to true.</remarks>
    public interface IMyModel : IModelBase
    {
        /// <summary>
        /// Gets or sets MyFirstProperty that is the first property of the model example.
        /// </summary>
        /// <remarks>If the property is set, the IsDirty flag will be true.</remarks>
        string MyFirstProperty { get; set; }

        /// <summary>
        /// Gets or sets MySecondProperty that is the second property of the model example.
        /// </summary>
        double MySecondProperty { get; set; }
    }
```

The GeneratorTools project provides you the tools to help you to generate the model implementation based on
a pattern you can define in C# :

* The Pattern model interface

```csharp
    /// <summary>
    /// Model pattern interface used by the Model pattern implementation.
    /// </summary>
    public interface IModelPattern : IModelBase
    {
        /// <summary>
        /// Gets or sets property declaration pattern.
        /// </summary>
        object PropertyPattern { get; set; }
    }
```

* The Pattern model implementation

```csharp
    /// <summary>
    /// Model pattern implementation.
    /// </summary>
    public class ModelPattern : IModelPattern
    {
        private object propertyPattern;

        /// <inheritdoc/>
        public bool IsDirty
        { get; private set; }

        /// <inheritdoc/>
        public object PropertyPattern
        {
            get
            {
                return this.propertyPattern;
            }

            set
            {
                this.propertyPattern = null;
                this.IsDirty = true;
            }
        }
    }
```

The result of the generated implementation for `IMyModel` is going to be like this:

```csharp
    /// <summary>
    /// Model pattern implementation.
    /// </summary>
    public class MyModel : IMyModel
    {
        private string myFirstProperty;
        private double mySecondProperty;

        /// <inheritdoc/>
        public bool IsDirty
        { get; private set; }

        /// <inheritdoc/>
        public string MyFirstProperty
        {
            get
            {
                return this.myFirstProperty;
            }

            set
            {
                this.myFirstProperty = value;
                this.IsDirty = true;
            }
        }

        /// <inheritdoc/>
        public double MySecondProperty
        {
            get
            {
                return this.mySecondProperty;
            }

            set
            {
                this.mySecondProperty = value;
                this.IsDirty = true;
            }
        }
    }
```

### Dependency injection

First if you are using dependency injection, you need to use the method extension to register the services:

```csharp
using SoloX.GeneratorTools.Core.CSharp;

void Setup(IServiceCollection serviceCollection)
{
    serviceCollection.AddCSharpToolsGenerator();
}
```

### Parse your existing C# projects and source files

In order to load and parse a C# project you need to use a `SoloX.GeneratorTools.Core.CSharp.Workspace.ICSharpWorkspace`.

Let's define a `ModelGenerator` with a `ICSharpWorkspace` constructor argument:

```csharp
    /// <summary>
    /// The model generator.
    /// </summary>
    public class ModelGenerator
    {
        private ICSharpWorkspace workspace;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGenerator"/> class.
        /// </summary>
        /// <param name="workspace">The workspace to use to load the project data.</param>
        public ModelGeneratorExample(ICSharpWorkspace workspace)
        {
            this.workspace = workspace;
        }
    }
```

Now we can use the `workspace` to register a project with the pattern we need and we can load the sources.

```csharp
    // First we need to register the project.
    this.workspace.RegisterProject(projectFile);

    // Register the pattern interface.
    var patternInterfaceDeclaration = this.workspace.RegisterFile("./Patterns/Itf/IModelPattern.cs")
        .Declarations.Single() as IInterfaceDeclaration;

    // Register the pattern implementation.
    var patternImplementationDeclaration = this.workspace.RegisterFile("./Patterns/Impl/ModelPattern.cs")
        .Declarations.Single() as IGenericDeclaration;

    // Load the project and its project dependencies. (Note that for now we only load the sources.
    // The binary assembly dependencies are not taken into account)
    var resolver = this.workspace.DeepLoad();
```

Once all is loaded we can get the `IModelBase` descriptor and get all interfaces extending it:

```csharp
    // Get the base interface in order to find all extended interfaces that need to be implemented.
    var modelBaseInterface = resolver.Find("IModelBase").Single() as IGenericDeclaration;

    var allModelInterfaces = modelBaseInterface.ExtendedBy;
```

### Generate your class implementation from a given interface and the pattern

The GeneratorTools project provides a `ImplementationGenerator` that can generate a class implementation from
the pattern and the model interface:

```csharp
    // Create the Implementation Generator with a file generator, the locator and the pattern interface/class.
    var generator = new ImplementationGenerator(
        // Tells that we want to write the implementation in a file.
        new FileGenerator(".generated.cs"),
        // Tells that we want the implementation class to be located at the same location than its model interface.
        new RelativeLocator(projectFolder, projectNameSpace),
        // The pattern interface we loaded previously.
        patternInterfaceDeclaration,
        // The pattern implementation we loaded previously.
        patternImplementationDeclaration);
```

With this created instance we can call the `Generate` method with the model interface we want to implement and the name of the implementation class.
It also requires a `WriterSelector` initialized with a `INodeWriter` collection:

```csharp
    // Loop on all interface extending the base model interface.
    foreach (var modelInterface in allModelInterfaces)
    {
        var implName = GeneratorHelper.ComputeClassName(modelInterface.Name);

        // Create the property writer what will use all properties from the model interface to generate
        // and write the corresponding code depending on the given pattern property.
        var propertyWriter = new PropertyWriter(
            patternInterfaceDeclaration.Properties.Single(),
            modelInterface.Properties);

        // Setup some basic text replacement writer.
        var itfNameWriter = new StringReplaceWriter(patternInterfaceDeclaration.Name, modelInterface.Name);
        var implNameWriter = new StringReplaceWriter(patternImplementationDeclaration.Name, implName);

        // Create the writer selector.
        var writerSelector = new WriterSelector(propertyWriter, itfNameWriter, implNameWriter);

        // And generate the class implementation.
        generator.Generate(writerSelector, (IInterfaceDeclaration)modelInterface, implName);
    }
```

