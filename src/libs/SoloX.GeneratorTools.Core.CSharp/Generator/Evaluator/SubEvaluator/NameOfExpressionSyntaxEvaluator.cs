﻿// ----------------------------------------------------------------------
// <copyright file="NameOfExpressionSyntaxEvaluator.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SoloX.GeneratorTools.Core.CSharp.Generator.Evaluator.SubEvaluator
{
    internal class NameOfExpressionSyntaxEvaluator : CSharpSyntaxVisitor<string>
    {
        public override string VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            return this.Visit(node.Name);
        }

        public override string VisitArgument(ArgumentSyntax node)
        {
            return this.Visit(node.Expression);
        }

        public override string VisitIdentifierName(IdentifierNameSyntax node)
        {
            return node.Identifier.ValueText;
        }
    }
}
