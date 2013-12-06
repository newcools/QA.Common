// -----------------------------------------------------------------------
// <copyright file="TestUtilities.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Automation.Common.Testing
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Automation.Common.Testing.Entity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The utilities for test entities managements, or to facilitate test execution.
    /// </summary>
    public static class TestUtilities
    {
        #region Public Methods and Operators

        /// <summary>
        /// The reflection type load exception.
        /// </summary>
        /// <param name="exception">
        /// The exception.
        /// </param>
        public static void CheckReflectionTypeLoadException(Exception exception)
        {
            if (exception == null)
            {
                return;
            }
            Exception objErr = exception.GetBaseException();

            var reflErr = objErr as ReflectionTypeLoadException;
            if (reflErr != null)
            {
                foreach (Exception ex in reflErr.LoaderExceptions)
                {
                    Debug.WriteLine(ex.Message, "Load exception");
                }
            }
        }

        /// <summary>
        /// To copy the test results to the specified destination. i.e. @"C:\drops\TestResult"
        /// </summary>
        /// <param name="result">
        /// The current test result.
        /// </param>
        /// <param name="destination">
        /// The destination.
        /// </param>
        public static void CopyTo(this TestResult result, string destination = null)
        {
            if (result == null || string.IsNullOrWhiteSpace(destination))
            {
                return;
            }

            var resultFile = new FileInfo(result.ResultFile);

            string destFileName = Path.Combine(destination, resultFile.Name);
            resultFile.CopyTo(destFileName, true);
        }

        /// <summary>
        /// The get test class name.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string FullyQualifiedTestCaseName(this TestContext testContext)
        {
            if (testContext == null)
            {
                throw new ArgumentNullException("testContext", "TestContext can not be null.");
            }

            string fullyQualifiedTestClassName = testContext.FullyQualifiedTestClassName;
            string testName = testContext.TestName;
            return string.Format("{0}.{1}", fullyQualifiedTestClassName, testName);
        }

        /// <summary>
        /// Gets the name space from the current test context.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetNameSpace(this TestContext testContext)
        {
            if (testContext == null)
            {
                throw new ArgumentNullException("testContext", "TestContext can not be null.");
            }

            string[] nameSections = testContext.FullyQualifiedTestClassName.Split('.');
            return nameSections.Take(nameSections.Length - 1)
                .Aggregate((result, next) => string.Format("{0}.{1}", result, next)).TrimStart('.');
        }

        /// <summary>
        /// The get stamped file name.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public static string GetStampedFileName(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (fileInfo == null)
            {
                throw new InvalidOperationException();
            }
            string extension = fileInfo.Extension;
            string nameOnly = fileInfo.Name.Replace(extension, string.Empty);
            string stampedName = string.Format(
                "{0}-{1}{2}", nameOnly, DateTime.Now.ToString("dd-MM-yyyy HHmmss"), extension);

            // ReSharper disable AssignNullToNotNullAttribute
            return Path.Combine(fileInfo.DirectoryName, stampedName);

            // ReSharper restore AssignNullToNotNullAttribute
        }

        /// <summary>
        /// The get test class name.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetTestClassName(this TestContext testContext)
        {
            if (testContext == null)
            {
                throw new ArgumentNullException("testContext", "TestContext can not be null.");
            }

            string fullName = testContext.FullyQualifiedTestClassName;
            return fullName.Split('.').LastOrDefault();
        }

        #endregion
    }
}