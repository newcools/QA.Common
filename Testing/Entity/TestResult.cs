namespace Automation.Common.Testing.Entity
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    ///     The test result.
    /// </summary>
    public class TestResult : EntityBase
    {
        #region Static Fields

        /// <summary>
        ///     The test projects.
        /// </summary>
        private static readonly Dictionary<string, TestResult> Results = new Dictionary<string, TestResult>();

        #endregion

        #region Fields

        /// <summary>
        ///     The _document.
        /// </summary>
        private readonly XDocument document;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestResult"/> class.
        /// </summary>
        /// <param name="resultFile">
        /// The result file.
        /// </param>
        private TestResult(string resultFile)
        {
            if (resultFile == null)
            {
                throw new ArgumentNullException("resultFile");
            }

            this.ResultFile = resultFile;
            Init(resultFile);
            this.document = XDocument.Load(resultFile);
            this.Projects = new List<TestProject>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the result file.
        /// </summary>
        public string ResultFile { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the projects.
        /// </summary>
        internal ICollection<TestProject> Projects { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// To either get an existing test project, or create a new test project if does not exist.
        /// </summary>
        /// <param name="projectFullName">
        /// The project full name.
        /// </param>
        /// <returns>
        /// The <see cref="TestProject"/> matches the specified project full name.
        /// </returns>
        public TestProject GetTestProject(string projectFullName)
        {
            if (string.IsNullOrWhiteSpace(projectFullName))
                throw new ArgumentException("Project name cannot be null or empty.", "projectFullName");
            
            TestProject testProject = this.Projects.FirstOrDefault(project => project.FullyQualifiedName.Equals(projectFullName));
            return testProject 
                ?? new TestProject { TestResult = this, FullyQualifiedName = projectFullName };
        }

        /// <summary>
        /// Initialize an instance of <see cref="TestResult"/> from test result file name.
        /// </summary>
        /// <param name="fullyQualifiedName">
        /// The fully qualified name of the test result.
        /// </param>
        /// <returns>
        /// The <see cref="TestResult"/> instance.
        /// </returns>
        public static TestResult InitTestResult(string fullyQualifiedName)
        {
            if (string.IsNullOrWhiteSpace(fullyQualifiedName))
            {
                throw new ArgumentException("Test result FQDN cannot be null or empty", "fullyQualifiedName");
            }

            if (Results.ContainsKey(fullyQualifiedName))
            {
                return Results[fullyQualifiedName];
            }

            TestResult testResult = new TestResult(fullyQualifiedName) { FullyQualifiedName = fullyQualifiedName };
            Results.Add(fullyQualifiedName, testResult);
            return testResult;
        }

        /// <summary>
        ///     Update all information to result elements, and save into the result file.
        /// </summary>
        public static void SaveAll()
        {
            Results.Select(result => result.Value).ForEach(Save);
        }

        /// <summary>
        ///     Save this result content into the result file.
        /// </summary>
        public void Save()
        {
            this.Projects.ForEach(UpdateProjectElement);
            SaveContent(this.document, this.ResultFile);
        }

        #endregion

        #region Methods

        /// <summary>
        /// To construct an entity xml element.
        /// </summary>
        /// <param name="entity">
        /// The entity object.
        /// </param>
        /// <param name="elementName">
        /// The element name.
        /// </param>
        /// <returns>
        /// The entity element.
        /// </returns>
        private static XElement ConstructEntityXElement(EntityBase entity, string elementName)
        {
            return new XElement(
                elementName,
                new XAttribute(Constants.NameAttribute, entity.Name),
                new XAttribute(Constants.StatusAttribute, entity.Status.ToString()),
                new XAttribute(Constants.StartTimeAttribute, entity.StartTime ?? DateTimeAdapter.Now),
                new XAttribute(Constants.EndTimeAttribute, entity.EndTime ?? string.Empty));
        }

        /// <summary>
        /// The get entity.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="childElement">
        /// The child Element.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        private static XElement GetEntityElement(EntityBase entity, XContainer parent, string childElement)
        {
            IEnumerable<XElement> entities = from c in parent.Descendants(childElement)
                                             let nameAttribute = c.Attribute(Constants.NameAttribute)
                                             where nameAttribute != null && nameAttribute.Value.Equals(entity.Name)
                                             select c;
            return entities.FirstOrDefault();
        }

        /// <summary>
        /// Initialize the specified result file. Create the result file if does not exist yet.
        /// </summary>
        /// <param name="resultFile">
        /// The result file name.
        /// </param>
        private static void Init(string resultFile)
        {
            if (File.Exists(resultFile))
            {
                return;
            }

            var document = new XDocument();
            document.Add(new XProcessingInstruction(Constants.StyleSheetInstruction, Constants.StyleSheetData));
            document.Add(new XElement(Constants.Root));

            var file = new FileInfo(resultFile);
            string directory = file.DirectoryName;

            // ReSharper disable AssignNullToNotNullAttribute
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // ReSharper restore AssignNullToNotNullAttribute
            SaveContent(document, resultFile);
        }

        /// <summary>
        /// Initialize the test case.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// </exception>
        private static XElement InitTestCase(TestCase entity)
        {
            if (entity.TestClass == null)
            {
                throw new ArgumentNullException(
                    "entity", string.Format("Test case [{0}] is not linked with a test class.", entity.Name));
            }

            // Get the parent element from loaded xml result file
            XElement parent = InitTestClass(entity.TestClass);

            XElement testCaseElement = GetEntityElement(entity, parent, Constants.TestCaseElement);

            if (testCaseElement == null)
            {
                testCaseElement = ConstructEntityXElement(entity, Constants.TestCaseElement);

                parent.Add(testCaseElement);
            }

            return testCaseElement;
        }

        /// <summary>
        /// Initialize the test class.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// </exception>
        private static XElement InitTestClass(TestClass entity)
        {
            if (entity.TestProject == null)
            {
                throw new ArgumentNullException(
                    "entity", string.Format("Test class [{0}] is not linked with a test project.", entity.Name));
            }

            // Get the parent element from loaded xml result file
            XElement parent = InitTestProject(entity.TestProject);

            XElement testClassElement = GetEntityElement(entity, parent, Constants.ClassElement);

            if (testClassElement == null)
            {
                testClassElement = ConstructEntityXElement(entity, Constants.ClassElement);

                parent.Add(testClassElement);
            }

            return testClassElement;
        }

        /// <summary>
        /// Initialize the test project.
        /// </summary>
        /// <param name="entity">
        /// The test project.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// </exception>
        private static XElement InitTestProject(TestProject entity)
        {
            if (entity.TestResult == null)
            {
                throw new InvalidOperationException(
                    string.Format("Test project [{0}] is not linked with a test result.", entity.Name));
            }

            TestResult testResult = entity.TestResult;

            // Get the root from loaded xml result file
            XElement parent = testResult.document.Root;

            if (parent == null)
            {
                throw new InvalidDataException("Test document root was not initialized properly.");
            }

            XElement project = GetEntityElement(entity, parent, Constants.ProjectElement);

            if (project == null)
            {
                project = ConstructEntityXElement(entity, Constants.ProjectElement);

                parent.Add(project);
            }

            return project;
        }

        /// <summary>
        /// Initialize the test scenario.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="XElement"/>.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// </exception>
        private static XElement InitTestScenario(Scenario entity)
        {
            if (entity.TestCase == null)
            {
                throw new ArgumentNullException(
                    "entity", string.Format("Test scenario [{0}] is not linked with a test case.", entity.Name));
            }

            // Get the parent element from loaded xml result file
            XElement parent = InitTestCase(entity.TestCase);

            XElement scenarioElement = GetEntityElement(entity, parent, Constants.ScenarioElement);

            if (scenarioElement == null)
            {
                scenarioElement = ConstructEntityXElement(entity, Constants.ScenarioElement);

                parent.Add(scenarioElement);
            }

            return scenarioElement;
        }

        /// <summary>
        /// To save the test result into the result file.
        /// </summary>
        /// <param name="result">
        /// The test result entity.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static void Save(TestResult result)
        {
            if (string.IsNullOrEmpty(result.ResultFile))
            {
                throw new InvalidOperationException("Result file path is not set properly.");
            }

            result.Save();
        }

        /// <summary>
        /// The save result.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="resultFile">
        /// The result file.
        /// </param>
        private static void SaveContent(XDocument document, string resultFile)
        {
            var settings = new XmlWriterSettings { Indent = true, IndentChars = "  ", NewLineOnAttributes = false };
            using (XmlWriter writer = XmlWriter.Create(resultFile, settings))
            {
                document.Save(writer);
                writer.Flush();
            }
        }

        /// <summary>
        /// Update test entity.
        /// </summary>
        /// <param name="testEntity">
        /// The test entity.
        /// </param>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <exception cref="NullReferenceException">
        /// </exception>
        private static void UpdateEntityElement(EntityBase testEntity, XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element", "Element is not initialized.");
            }

            if (testEntity == null)
            {
                throw new ArgumentNullException("testEntity", "Test entity can not be null.");
            }

            element.SetAttributeValue(Constants.StatusAttribute, testEntity.Status.ToString());
            element.SetAttributeValue(Constants.StartTimeAttribute, testEntity.StartTime ?? string.Empty);
            element.SetAttributeValue(Constants.EndTimeAttribute, testEntity.EndTime ?? DateTimeAdapter.Now);
        }

        /////// <summary>
        /////// Saves the project into result file.
        /////// </summary>
        /////// <param name="testProject">
        /////// The test project.
        /////// </param>
        ////private void SaveProject(TestProject testProject)
        ////{
        ////    UpdateProjectElement(testProject);
        ////    SaveContent(this.document, this.ResultFile);
        ////}

        /// <summary>
        /// Updates the project element in this instance.
        /// </summary>
        /// <param name="testProject">
        /// The test project.
        /// </param>
        private static void UpdateProjectElement(TestProject testProject)
        {
            if (testProject == null)
            {
                throw new ArgumentNullException("testProject", "Cannont save null project to test result.");
            }

            if (testProject.Saved)
            {
                return;
            }

            XElement projectElement = InitTestProject(testProject);
            UpdateEntityElement(testProject, projectElement);

            // Update testClasses of this test project
            testProject.TestClasses.ForEach(UpdateTestClassElement);
            testProject.Saved = true;
        }

        /// <summary>
        /// Updates the scenario element in this instance.
        /// </summary>
        /// <param name="scenario">
        /// The scenario.
        /// </param>
        private static void UpdateScenarioElement(Scenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException("scenario", "Cannont save null test scenario to test result.");
            }

            if (scenario.Saved)
            {
                return;
            }

            XElement scenarioElement = InitTestScenario(scenario);
            UpdateEntityElement(scenario, scenarioElement);
            scenario.Messages.ForEach(message => UpdateTestEntityMessageElement(message, scenarioElement));
            scenario.Saved = true;
        }

        /// <summary>
        /// Updates the test case element in this instance.
        /// </summary>
        /// <param name="testCase">
        /// The test case.
        /// </param>
        private static void UpdateTestCaseElement(TestCase testCase)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException("testCase", "Cannont save null test case to test result.");
            }

            if (testCase.Saved)
            {
                return;
            }

            XElement testCaseElement = InitTestCase(testCase);
            UpdateEntityElement(testCase, testCaseElement);
            testCase.Messages.ForEach(message => UpdateTestEntityMessageElement(message, testCaseElement));
            testCase.Scenarios.ForEach(UpdateScenarioElement);
            testCase.Saved = true;
        }

        /// <summary>
        /// Updates the test class element in this instance.
        /// </summary>
        /// <param name="testClass">
        /// The test class.
        /// </param>
        private static void UpdateTestClassElement(TestClass testClass)
        {
            if (testClass == null)
            {
                throw new ArgumentNullException("testClass", "Cannont save null test class to test result.");
            }

            if (testClass.Saved)
            {
                return;
            }

            XElement testClassElement = InitTestClass(testClass);
            UpdateEntityElement(testClass, testClassElement);

            // Update all children test cases
            testClass.TestCases.ForEach(UpdateTestCaseElement);
            testClass.Saved = true;
        }

        /// <summary>
        /// The update test entity message element.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="testEntityElement">
        /// The test entity element. Either a test case or a test scenario.
        /// </param>
        private static void UpdateTestEntityMessageElement(Message message, XElement testEntityElement)
        {
            XElement messageElement = new XElement(Constants.MessageElement, message.Description);
            messageElement.SetAttributeValue(Constants.CreateTime, message.Time);
            testEntityElement.Add(messageElement);
        }

        #endregion
    }
}