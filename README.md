# GeneratorTools [![CircleCI](https://circleci.com/gh/xaviersolau/GeneratorTools.svg?style=svg)](https://circleci.com/gh/xaviersolau/GeneratorTools) [![Coverage Status](https://coveralls.io/repos/github/xaviersolau/GeneratorTools/badge.svg?branch=master)](https://coveralls.io/github/xaviersolau/GeneratorTools?branch=master) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

GeneratorTools is a project that helps you to automate C# code generation.
It is written in C# and thanks to .Net Standard, it is cross platform.

It also provides you with an API to parse your existing C# sources (based on Roslyn).

Don't hesitate to post issue, pull request on the project or to fork and improve the project.

## License and credits

GeneratorTools project is written by Xavier Solau. It's licensed under the MIT license.

 * * *

## Installation

You can checkout this Github repository or use the NuGet package that will be available soon.

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

The result of the generated implementation for `IMyModel` could be something like this:

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

### Parse your existing C# projects and source files


### Generate your class implementation from a given interface and the pattern



