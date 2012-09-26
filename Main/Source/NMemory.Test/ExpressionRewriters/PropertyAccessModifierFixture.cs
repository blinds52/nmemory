﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMemory.Execution.Optimization.Rewriters;
using NMemory.Execution.Optimization;
using System.Linq.Expressions;
using NMemory.Test.Environment.Data;
using NMemory.Test.Environment.Utils;

namespace NMemory.Test.ExpressionRewriters
{
    [TestClass]
    public class PropertyAccessModifierFixture
    {
        [TestMethod]
        public void PropertyAccessModiferOnNullableMember()
        {
            IExpressionRewriter modifier = new PropertyAccessRewriter();
            Expression<Func<Member, int?>> expression = m => m.GroupId;

            Expression newBody = modifier.ModifyExpression(expression.Body);
            expression = ExpressionUtils.ChangeBody(expression, newBody);

            int? result = expression.Compile().Invoke(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PropertyAccessModiferOnMappedNullableMember()
        {
            IExpressionRewriter modifier = new PropertyAccessRewriter();
            Expression<Func<Member, string>> expression = m => m.GroupId == null ? "null" : "not null";

            Expression newBody = modifier.ModifyExpression(expression.Body);
            expression = ExpressionUtils.ChangeBody(expression, newBody);

            string result = expression.Compile().Invoke(null);

            Assert.AreEqual("null", result);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PropertyAccessModiferOnNotNullableMember()
        {
            IExpressionRewriter modifier = new PropertyAccessRewriter();
            Expression<Func<Member, int>> expression = m => m.GroupId2;

            Expression newBody = modifier.ModifyExpression(expression.Body);
            expression = ExpressionUtils.ChangeBody(expression, newBody);

            int? result = expression.Compile().Invoke(null);
        }

        [TestMethod]
        public void PropertyAccessModiferOnCastedMember()
        {
            IExpressionRewriter modifier = new PropertyAccessRewriter();
            Expression<Func<Member, int?>> expression = m => (int?)m.GroupId2;

            Expression newBody = modifier.ModifyExpression(expression.Body);
            expression = ExpressionUtils.ChangeBody(expression, newBody);

            int? result = expression.Compile().Invoke(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void PropertyAccessModiferOnCastedNullableMember()
        {
            IExpressionRewriter modifier = new PropertyAccessRewriter();
            Expression<Func<Member, int?>> expression = m => (int?)m.GroupId;

            Expression newBody = modifier.ModifyExpression(expression.Body);
            expression = ExpressionUtils.ChangeBody(expression, newBody);

            int? result = expression.Compile().Invoke(null);

            Assert.IsNull(result);
        }
    }
}