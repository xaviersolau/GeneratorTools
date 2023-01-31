# GeneratorTools

GeneratorTools is a project that helps you to automate C# code generation.
It is written in C# and thanks to .Net Standard, it is cross platform.

It also provides you with an API to parse your existing C# sources (based on Roslyn).

Don't hesitate to post issue, pull request on the project or to fork and improve the source code.

## Project dashboard

[![Build - CI](https://github.com/xaviersolau/GeneratorTools/actions/workflows/build-ci.yml/badge.svg)](https://github.com/xaviersolau/GeneratorTools/actions/workflows/build-ci.yml)
[![Coverage Status](https://coveralls.io/repos/github/xaviersolau/GeneratorTools/badge.svg?branch=master)](https://coveralls.io/github/xaviersolau/GeneratorTools?branch=master)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![NuGet Beta](https://img.shields.io/nuget/vpre/SoloX.GeneratorTools.Core.CSharp.svg)](https://www.nuget.org/packages/SoloX.GeneratorTools.Core.CSharp)

## License and credits

GeneratorTools project is written by Xavier Solau. It's licensed under the MIT license.

 * * *

## Installation

You can checkout this Github repository or you can use the NuGet package:

**Install using the command line from the Package Manager:**
```bash
Install-Package SoloX.GeneratorTools.Core.CSharp -version 1.0.0-alpha.26
```

**Install using the .Net CLI:**
```bash
dotnet add package SoloX.GeneratorTools.Core.CSharp --version 1.0.0-alpha.26
```

**Install editing your project file (csproj):**
```xml
<PackageReference Include="SoloX.GeneratorTools.Core.CSharp" Version="1.0.0-alpha.26" />
```

## The use case

> Note that you can find code examples in this repository in this location: [`src/examples`](src/examples).

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

The `IModelBase` interface defines a `IsDirty` property that is going to be `true` once a model implementation
property is set.

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

The GeneratorTools project provides you the tools to help you to generate the implementation for a given interface based on
a pattern you can define in C# :

* The Pattern interface

Basically, this is the simplest example of the interface you want the user to write. Here we have only one
property defined in the interface.

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

* The Pattern implementation

The pattern implementation is the code you want the tool to generate for the pattern interface.
So in our example it will define a field and a property that is updating the IsDirty flag when
it is modified.

```csharp
    /// <summary>
    /// Model pattern implementation that should be generated for the previous pattern model interface.
    /// </summary>
    public class ModelPattern : IModelPattern
    {
        /// <summary>
        /// The IsDirty implementation.
        /// </summary>
        public bool IsDirty
        { get; private set; }

        /// <summary>
        /// The field used be the property implementation.
        /// </summary>
        private object propertyPattern;

        /// <summary>
        /// The actual property implementation that is updating IsDirty on set.
        /// </summary>
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

We expected the tool to generate implementations for all interfaces based on IModelBase. It will generate the field and
the property code replacing the pattern name with the real name of the property. And the generator is going to repeat
this code generation for every property defined in the interface.

In order to inform the generator tool if a code must be repeated or not, we have to add some attributes in
the implementation pattern.

Let's get back to our pattern implementation example with the attributes added:

```csharp
    /// <summary>
    /// Here we need two attributes:
    /// * The Pattern attribute tells the generator how it can find the interfaces from witch it must generate the
    ///  implementation based on the implementation pattern.
    ///  Here we use typeof(InterfaceBasedOnSelector<IModelBase>) to tell the generator to generate code for all
    ///  interfaces extending the IModelBase interface.
    ///  
    /// * The Repeat attribute means that the generator must repeat the pattern model implementation on each
    ///  interfaces targeted by the Pattern attribute.
    ///  The Pattern argument given in the Repeat attribute allows the generator to make the link with the interface
    ///  pattern model.
    ///  The argument Prefix given in the Repeat attribute allows the generator to make text replacement from the
    ///  pattern model implementation to the generated code. Here it will replace all IModelPattern text by the real
    ///  name of the targeted interface.
    ///  
    /// </summary>
    [Pattern(typeof(InterfaceBasedOnSelector<IModelBase>))]
    [Repeat(Pattern = nameof(IModelPattern), Prefix = "I")]
    public class ModelPattern : IModelPattern
    {
        /// <summary>
        /// We don't need any attribute since we just want the generator to copy the IsDirty property.
        /// </summary>
        public bool IsDirty
        { get; private set; }

        /// <summary>
        /// Here we need the Repeat attribute to tell the generator that we want to repeat the field for each
        /// Property defined in the targeted interface. The Property are selected matching the one in the pattern
        /// model interface (In this case PropertyPattern in IModelPattern).
        /// </summary>
        [Repeat(Pattern = nameof(IModelPattern.PropertyPattern))]
        private object propertyPattern;

        /// <summary>
        /// Here again we need the Repeat attribute to tell the generator that we want to repeat the Property for each
        /// Property defined in the targeted interface.
        /// </summary>
        [Repeat(Pattern = nameof(IModelPattern.PropertyPattern))]
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

So the result of the generated implementation for `IMyModel` is going to be like this:

```csharp
    /// <summary>
    /// Model implementation.
    /// </summary>
    public class MyModel : IMyModel
    {
        public bool IsDirty
        { get; private set; }

        private string myFirstProperty;
        private double mySecondProperty;

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

## How to use it

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

In order to load and parse a C# project you need to use a `SoloX.GeneratorTools.Core.CSharp.Workspace.ICSharpWorkspace`
created from the factory `SoloX.GeneratorTools.Core.CSharp.Workspace.ICSharpWorkspaceFactory`.

Let's define a `ModelGenerator` with a `ICSharpWorkspaceFactory` constructor argument:

```csharp
    /// <summary>
    /// The model generator.
    /// </summary>
    public class ModelGenerator
    {
        private ICSharpWorkspaceFactory workspaceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelGenerator"/> class.
        /// </summary>
        /// <param name="workspaceFactory">The workspace to use to load the project data.</param>
        public ModelGenerator(ICSharpWorkspaceFactory workspaceFactory)
        {
            this.workspaceFactory = workspaceFactory;
        }
    }
```

Now we can create and use the `workspace` to register a project with the pattern we need and we can load the sources.

```csharp
    // Create a Workspace
    var workspace = this.workspaceFactory.CreateWorkspace();

    // First we need to register the project.
    workspace.RegisterProject(projectFile);

    // Register the pattern interface.
    var patternInterfaceDeclaration = this.workspace.RegisterFile("./Patterns/Itf/IModelPattern.cs")
        .Declarations.Single() as IInterfaceDeclaration;

    // Register the pattern implementation.
    var patternImplementationDeclaration = this.workspace.RegisterFile("./Patterns/Impl/ModelPattern.cs")
        .Declarations.Single() as IGenericDeclaration<SyntaxNode>;

    // Load the project and its project dependencies. (Note that we load the sources and the binary assembly
    // dependencies. Both are taken into account)
    var resolver = this.workspace.DeepLoad();
```

### Generate your class implementation from a given project files and the pattern

The GeneratorTools project provides a `AutomatedGenerator` that can generate all class implementations from
the pattern and the project files:

```csharp
    // Create the `AutomatedGenerator` with a file generator, the locator and the pattern interface/class.
    var generator = new AutomatedGenerator(
        // Tells that we want to write the implementation in a file.
        new FileGenerator(".generated.cs"),
        // Tells that we want the implementation class to be located at the same location than its model interface.
        new RelativeLocator(projectFolder, projectNameSpace),
        // The target project type resolver. (previously created)
        resolver,
        // The pattern type.
        typeof(ModelPattern),
        // A Logger forwarding logs to the given `ILogger`.
        new GeneratorLogger<ModelGeneratorExample>(this.logger));
```

With this created generator instance we can call the `Generate` method with the set of files where to apply the
pattern selector to get all interfaces to generate.

```csharp
    // Generate the implementations from the given files.
    generator.Generate(project.Files);
```

