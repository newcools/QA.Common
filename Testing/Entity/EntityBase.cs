namespace Automation.Common.Testing.Entity
{
    /// <summary>
    /// The base class to be derived by all test entity types.
    /// </summary>
    public abstract class EntityBase
    {
        #region Fields

        /// <summary>
        /// The _end time.
        /// </summary>
        private string endTime = string.Empty;

        /// <summary>
        /// The _name.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// The _start time.
        /// </summary>
        private string startTime = DateTimeAdapter.Now;

        /// <summary>
        /// The _status.
        /// </summary>
        private Status status = Status.NotStarted;

        /// <summary>
        /// The full qualified name.
        /// </summary>
        private string fullyQualifiedName = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityBase"/> class.
        /// </summary>
        protected EntityBase()
        {
            this.Saved = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public string EndTime
        {
            get
            {
                return this.endTime;
            }
            set
            {
                this.endTime = value;
                this.Saved = false;
            }
        }

        /// <summary>
        /// Gets or sets the full qualified name.
        /// </summary>
        public string FullyQualifiedName
        {
            get
            {
                return this.fullyQualifiedName;
            }
            set
            {
                this.fullyQualifiedName = value;
                this.Saved = false;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The entity name.
        /// </value>
        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.name))
                {
                    this.name = this.fullyQualifiedName;
                }
                return this.name;
            }
            set
            {
                this.name = value;
                this.Saved = false;
            }
        }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public virtual EntityBase Parent { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public string StartTime
        {
            get
            {
                return this.startTime;
            }
            protected set
            {
                this.startTime = value;
                this.Saved = false;
            }
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public Status Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                this.Saved = false;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EntityBase"/> is saved.
        /// </summary>
        /// <value>
        /// <c>true</c> if saved; otherwise, <c>false</c>.
        /// </value>
        internal bool Saved { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines if the current test entity is the same as the target test entity.
        /// </summary>
        /// <param name="target">
        /// The target test entity.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> value indicating whether the current test entity is the same as the target test entity.
        /// </returns>
        public bool Equals(EntityBase target)
        {
            return target != null && target.FullyQualifiedName.Equals(this.FullyQualifiedName);
        }

        /// <summary>
        /// Sets the end time to current time.
        /// </summary>
        public virtual void SetEndTime()
        {
            this.EndTime = DateTimeAdapter.Now;
        }

        /// <summary>
        /// Sets the status recursively.
        /// </summary>
        /// <param name="executionStatus">
        /// The status.
        /// </param>
        public virtual void SetStatus(Status executionStatus)
        {
            if (executionStatus == Status.PartiallyPass)
            {
                this.status = executionStatus;
                return;
            }
            if (this.Status == Status.NotStarted)
            {
                this.Status = executionStatus;
                return;
            }
            if (this.Status == Status.Pass && executionStatus == Status.Fail)
            {
                this.Status = Status.PartiallyPass;
                return;
            }

            if (this.Status == Status.Fail && executionStatus == Status.Pass)
            {
                this.Status = Status.PartiallyPass;
            }
        }

        #endregion
    }
}