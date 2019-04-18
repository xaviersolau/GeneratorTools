// ----------------------------------------------------------------------
// <copyright file="WriterSelectorTest.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Moq;
using SoloX.GeneratorTools.Core.Generator.Writer;
using SoloX.GeneratorTools.Core.Generator.Writer.Impl;
using Xunit;

namespace SoloX.GeneratorTools.Core.UTest.Generator
{
    public class WriterSelectorTest
    {
        [Theory]
        [InlineData(false, false, false)]
        [InlineData(false, true, true)]
        [InlineData(true, false, true)]
        [InlineData(true, true, true)]
        public void BasicWriterSelectorTest(bool w1, bool w2, bool expected)
        {
            var nw1 = SetupNodeWriter(w1, "1");
            var nw2 = SetupNodeWriter(w2, "2");

            var writerSelector = new WriterSelector(nw1, nw2);
            var resText = string.Empty;

            var res = writerSelector.SelectAndProcessWriter(null, (s) => resText = s);

            Assert.Equal(expected, res);
            if (w1)
            {
                Assert.Equal("1", resText);
            }
            else if (w2)
            {
                Assert.Equal("2", resText);
            }
            else
            {
                Assert.Equal(string.Empty, resText);
            }
        }

        [Fact]
        public void EmptyWriterSelectorTest()
        {
            var writerSelector = new WriterSelector();
            Assert.False(writerSelector.SelectAndProcessWriter(null, (s) => { }));
        }

        private static INodeWriter SetupNodeWriter(bool enable, string textToWrite)
        {
            var nodeWriterMock = new Mock<INodeWriter>();
            nodeWriterMock.Setup(w => w.Write(It.IsAny<SyntaxNode>(), It.IsAny<Action<string>>()))
                .Callback((SyntaxNode n, Action<string> a) =>
                {
                    if (enable)
                    {
                        a(textToWrite);
                    }
                })
                .Returns(enable);
            return nodeWriterMock.Object;
        }
    }
}
