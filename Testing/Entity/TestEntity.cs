namespace Automation.Common.Testing.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    ///     The test case.
    /// </summary>
    public abstract class TestEntity : EntityBase
    {
        #region Constants

        /// <summary>
        ///     The default fail message.
        /// </summary>
        private const string DefaultFailMessage = "Test failed. Please check the test report for more information.";

        #endregion

        #region Static Fields

        /// <summary>
        ///     The failures in row counter.
        /// </summary>
        private static int failuresInRowCounter;

        /// <summary>
        ///     The previous test status.
        /// </summary>
        private static Status previousTestStatus = Status.NotStarted;

        #endregion

        #region Fields

        /// <summary>
        ///     The test execution messages.
        /// </summary>
        private readonly IList<Message> messages;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestEntity" /> class.
        /// </summary>
        protected TestEntity()
        {
            this.messages = new List<Message>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the failures in row counter.
        /// </summary>
        public static int FailuresInRowCounter
        {
            get
            {
                return failuresInRowCounter;
            }
        }

        /// <summary>
        ///     Gets the messages.
        /// </summary>
        public IList<Message> Messages
        {
            get
            {
                return this.messages;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Resets failures in row counter to 0.
        /// </summary>
        public static void ResetFailuresInRowCounter()
        {
            failuresInRowCounter = 0;
        }

        /// <summary>
        /// The test case execution pattern.
        /// </summary>
        /// <param name="executionAction">
        /// The actual test steps to be executed.
        /// </param>
        /// <param name="cleanupAction">
        /// The clean up Action. Invoked at the end of the test execution if defined.
        /// </param>
        public void Execute(
            Action<TestEntity> executionAction,
            Action cleanupAction = default(Action))
        {
            if (executionAction == null)
            {
                throw new ArgumentNullException("executionAction", "A delegate is required to describe test steps.");
            }

            try
            {
                executionAction(this);
                if (this.Status == Status.NotStarted)
                {
                    this.SetStatus(Status.Pass);
                }

                Assert.IsTrue(this.Status == Status.Pass, DefaultFailMessage);
            }
            catch (Exception exception)
            {
                if (string.IsNullOrWhiteSpace(exception.Message))
                    this.LogError(exception.ToString());
                if (!exception.Message.Contains(DefaultFailMessage))
                {
                    this.LogError(exception.Message);
                }

                FailuresInRowUp();
                throw;
            }
            finally
            {
                this.SetEndTime();
                previousTestStatus = this.Status;
                if (cleanupAction != null)
                {
                    try
                    {
                        cleanupAction();
                    }
                    catch
                    {
                        // Do nothing, swallow the exception.
                        // The finally clean up action is to do house keeping work, such as 'restore states' if necessary.
                        // The house keeping failure should not affect the test status (i.e. the application's behavior).
                        this.LogWarning(string.Format("{0} clean up failed.", this.FullyQualifiedName));
                    }
                }
            }
        }

        /// <summary>
        /// Execute test procedures which expect TException.
        /// </summary>
        /// <param name="executionAction">
        /// The action to set up exception scenario.
        /// </param>
        /// <param name="message">
        /// The user defined error message to be logged when the expected exception is not thrown.
        /// </param>
        /// <param name="cleanupAction">
        /// Performs a clean up action if specified.
        /// </param>
        /// <typeparam name="TException">
        /// The expected exception type.
        /// </typeparam>
        public void ExpectException<TException>(
            Action<TestEntity> executionAction,
            string message = null,
            Action cleanupAction = default(Action))
            where TException : Exception
        {
            ExpectException<TException>(executionAction, null, message, cleanupAction);
        }

        /// <summary>
        /// Execute test procedures which expect TException.
        /// </summary>
        /// <param name="executionAction">
        /// The action to set up exception scenario.
        /// </param>
        /// <param name="exceptionValidator">
        /// The function to validate the exception contains expected information, such as messages or reasons.
        /// </param>
        /// <param name="message">
        /// The user defined error message to be logged when the expected exception is not thrown.
        /// </param>
        /// <param name="cleanupAction">
        /// Performs a clean up action if specified.
        /// </param>
        /// <typeparam name="TException">
        /// The expected exception type.
        /// </typeparam>
        public void ExpectException<TException>(
            Action<TestEntity> executionAction,
            Action<TException, TestEntity> exceptionValidator,
            string message = null,
            Action cleanupAction = default(Action))
            where TException : Exception
        {
            if (executionAction == null)
            {
                throw new ArgumentNullException("executionAction", "An action is required to set up the Exceptional scenario.");
            }

            try
            {
                executionAction(this);
                string errorMessage = message ?? string.Format("Expected exception of type [{0}] was not thrown.", typeof(TException));
                throw new AssertFailedException(errorMessage);
            }
            catch (TException expectedException)
            {
                if (exceptionValidator != null) exceptionValidator(expectedException, this);

                if (this.Status == Status.NotStarted) this.SetStatus(Status.Pass);

                Assert.IsTrue(this.Status == Status.Pass, DefaultFailMessage);
            }
            catch (AssertFailedException assertFailedException)
            {
                this.LogError(assertFailedException.Message);
                FailuresInRowUp();
                throw;
            }
            catch (Exception exception)
            {
                string failMessage =
                    message
                    ?? string.Format(
                        "Expected exception of type [{0}] was not thrown. Actual exception type was [{1}].",
                        typeof(TException),
                        exception.GetType());
                this.LogError(failMessage);
                FailuresInRowUp();
                throw;
            }
            finally
            {
                this.SetEndTime();
                previousTestStatus = this.Status;
                if (cleanupAction != null)
                {
                    try
                    {
                        cleanupAction();
                    }
                    catch
                    {
                        this.LogWarning(string.Format("{0} clean up failed.", this.FullyQualifiedName));
                    }
                }
            }
        }

        /// <summary>
        /// Initialize test prerequisites.
        /// </summary>
        /// <param name="initialise">
        /// The initialization steps.
        /// </param>
        public void Init(Action initialise)
        {
            if (initialise == null)
            {
                throw new ArgumentNullException("initialise", "A delegate is required to describe initialisation steps.");
            }

            try
            {
                initialise();
            }
            catch (Exception exception)
            {
                this.LogError(exception.Message);

                FailuresInRowUp();
                this.SetEndTime();
                previousTestStatus = Status.Fail;
                throw;
            }
        }

        /// <summary>
        /// Log an error message for the current test entity.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        public void LogError(string message)
        {
            this.LogMessage(MessageType.Error, message);
        }

        /// <summary>
        /// Log an error message for the current test entity.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void LogError(string message, params object[] parameters)
        {
            LogError(message, null, parameters);
        }

        /// <summary>
        /// Log an error message for the current test entity.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        /// <param name="defaultMessage">
        /// The default Message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void LogError(string message, string defaultMessage, params object[] parameters)
        {
            this.LogMessage(MessageType.Error, message, defaultMessage, parameters);
        }

        /// <summary>
        /// Log all messages as error.
        /// </summary>
        /// <param name="errorMessages">
        /// The messages to be logged.
        /// </param>
        public void LogErrors(IEnumerable<string> errorMessages)
        {
            if (errorMessages == null)
            {
                return;
            }

            errorMessages.ForEach(this.LogError);
        }

        /// <summary>
        /// Log a message for the current test entity as information.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        public void LogInfo(string message)
        {
            this.LogMessage(MessageType.Information, message);
        }

        /// <summary>
        /// Log a message for the current test entity as information.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void LogInfo(string message, params object[] parameters)
        {
            this.LogMessage(MessageType.Information, message, parameters);
        }

        /// <summary>
        /// Log the message as warning.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        public void LogWarning(string message)
        {
            this.LogMessage(MessageType.Warning, message);
        }

        /// <summary>
        /// Log the message as warning.
        /// </summary>
        /// <param name="message">
        /// The message to be logged.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void LogWarning(string message, params object[] parameters)
        {
            this.LogMessage(MessageType.Warning, message, parameters);
        }
        #endregion

        #region Methods

        /// <summary>
        ///     The failures in row goes up by 1.
        /// </summary>
        private static void FailuresInRowUp()
        {
            if (previousTestStatus == Status.Fail)
            {
                failuresInRowCounter = FailuresInRowCounter + 1;
                return;
            }

            failuresInRowCounter = 1;
        }

        /// <summary>
        /// Log the message for the current test entity.
        /// </summary>
        /// <param name="messageType">
        ///     Specify the message type.
        /// </param>
        /// <param name="message">
        ///     The message.
        /// </param>
        private void LogMessage(MessageType messageType, string message)
        {
            LogMessage(messageType, message, (object[])null);
        }

        /// <summary>
        /// Log the message for the current test entity.
        /// </summary>
        /// <param name="messageType">
        /// Specify the message type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        private void LogMessage(MessageType messageType, string message, params object[] parameters)
        {
            LogMessage(messageType, message, null, parameters);
        }

        /// <summary>
        /// Log the message for the current test entity.
        /// </summary>
        /// <param name="messageType">
        /// Specify the message type.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="defaultMessage">
        /// The default Message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        private void LogMessage(MessageType messageType, string message, string defaultMessage, params object[] parameters)
        {
            string completeMessage = CreateCompleteMessage(message, defaultMessage, parameters);

            this.Messages.Add(
                new Message
                {
                    Description = completeMessage,
                    MessageType = messageType
                });
            if (messageType == MessageType.Error)
            {
                this.SetStatus(Status.Fail);
            }

            this.Saved = false;
        }

        /// <summary>
        /// Creates a complete log message from the specified message, or default message and parameters.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="defaultMessage">
        /// The default Message.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> represents the full log message.
        /// </returns>
        private static string CreateCompleteMessage(string message, string defaultMessage, params object[] parameters)
        {
            string completeMessage;

            if (!string.IsNullOrWhiteSpace(message))
                completeMessage = parameters != null ? string.Format(CultureInfo.CurrentCulture, message, parameters) : message;
            else
            {
                completeMessage = defaultMessage ?? string.Empty;
            }
            return completeMessage;
        }
        #endregion
    }
}