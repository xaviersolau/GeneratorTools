// ----------------------------------------------------------------------
// <copyright file="AsyncMethodPattern.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.GeneratorTools.Core.CSharp.Generator.Attributes;
using SoloX.GeneratorTools.Core.CSharp.Generator.ReplacePattern;
using SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Itf;
using System.Threading.Tasks;

namespace SoloX.GeneratorTools.Core.CSharp.ITest.Generator.Automated.Patterns.Impl
{
    [Pattern<MultiSelector>]
    [Repeat(Pattern = nameof(IAsyncMethodPattern), Prefix = "I")]
    public class AsyncMethodPattern : IAsyncMethodPattern
    {
        [Repeat(Pattern = nameof(IAsyncMethodPattern.PatternMethodAsync))]
        [ReplacePattern(typeof(TaskValueTypeReplaceHandler))]
        public Task<object> PatternMethodAsync([Repeat(Pattern = "someArgument")] object someArgument)
        {
            return Task.FromResult<object>(default);
        }
    }
}
