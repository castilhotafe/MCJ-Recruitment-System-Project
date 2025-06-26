using System;


namespace TestProject1
{
    [TestClass]
    public class RecruitmentSystemUnitTests
    {
        private RecruitmentSystem system;

        [TestInitialize]
        public void Setup()
        {
            system = new RecruitmentSystem();
        }

        /// <summary>
        /// Valid contractor inputs should pass validation and be added.
        /// </summary>
        [TestMethod]
        public void AddContractor_ValidInputs_ContractorAdded()
        {
            // Arrange
            string first = "JOHN", last = "DOE", date = "2024-01-01", wage = "25.50";

            // Act
            string msg = system.VerifyInputs_AddContractor(first, last, date, wage);
            system.AddContractor(new Contractor(first, last, DateTime.Parse(date), decimal.Parse(wage)));

            // Assert
            Assert.IsNull(msg);
            Assert.AreEqual(1, system.GetAllContractors().Count);
        }

        /// <summary>
        /// Negative wage should return error message.
        /// </summary>
        [TestMethod]
        public void AddContractor_NegativeWage_ReturnsError()
        {
            // Arrange
            string first = "JANE", last = "DOE", date = "2024-01-01", wage = "-10";

            // Act
            string msg = system.VerifyInputs_AddContractor(first, last, date, wage);

            // Assert
            Assert.AreEqual("Hourly wage cannot be negative.", msg);
        }

        /// <summary>
        /// Remove contractor successfully and update job.
        /// </summary>
        [TestMethod]
        public void RemoveContractor_AssignedToJob_UnassignsFromJob()
        {
            // Arrange
            Contractor c = new Contractor("Anna", "Smith", DateTime.Now, 20);
            Job j = new Job("Test Job", DateTime.Today, 100);
            system.AddContractor(c);
            system.AddJob(j);
            system.AssignJob(j, c);

            // Act
            system.RemoveContractor(c);

            // Assert
            Assert.AreEqual(0, system.GetAllContractors().Count);
            Assert.IsNull(j.ContractorAssigned);
        }

        /// <summary>
        /// Remove contractor not assigned to any job.
        /// </summary>
        [TestMethod]
        public void RemoveContractor_NotAssigned_RemovedSuccessfully()
        {
            // Arrange
            Contractor c = new Contractor("Bob", "Brown", DateTime.Now, 20);
            system.AddContractor(c);

            // Act
            system.RemoveContractor(c);

            // Assert
            Assert.AreEqual(0, system.GetAllContractors().Count);
        }

        /// <summary>
        /// Get all contractors returns correct list.
        /// </summary>
        [TestMethod]
        public void GetAllContractors_ReturnsCorrectList()
        {
            // Arrange
            system.AddContractor(new Contractor("Tom", "Lee", DateTime.Now, 25));

            // Act
            List<Contractor> contractors = system.GetAllContractors();

            // Assert
            Assert.AreEqual(1, contractors.Count);
        }

        /// <summary>
        /// Get available contractors excludes assigned ones.
        /// </summary>
        [TestMethod]
        public void GetAvailableContractors_OneAssigned_ExcludesFromList()
        {
            // Arrange
            Contractor c1 = new Contractor("Tom", "Lee", DateTime.Now, 25);
            Contractor c2 = new Contractor("Sue", "Ray", DateTime.Now, 30);
            Job j = new Job("Fix", DateTime.Today, 100);
            system.AddContractor(c1);
            system.AddContractor(c2);
            system.AddJob(j);
            system.AssignJob(j, c1);

            // Act
            var available = system.GetAvailableContractors();

            // Assert
            Assert.AreEqual(1, available.Count);
            Assert.AreEqual(c2, available[0]);
        }

        /// <summary>
        /// Valid job input should be added.
        /// </summary>
        [TestMethod]
        public void AddJob_ValidInputs_JobAdded()
        {
            // Arrange
            string title = "PAINT", cost = "120", date = "2024-01-02";

            // Act
            string msg = system.VerifyInputs_AddJob(title, cost, DateTime.Parse(date));
            system.AddJob(new Job(title, DateTime.Parse(date), decimal.Parse(cost)));

            // Assert
            Assert.IsNull(msg);
            Assert.AreEqual(1, system.GetAllJobs().Count);
        }

        /// <summary>
        /// Negative job cost returns error.
        /// </summary>
        [TestMethod]
        public void AddJob_NegativeCost_ReturnsError()
        {
            // Arrange
            string title = "PAINT", cost = "-100";

            // Act
            string msg = system.VerifyInputs_AddJob(title, cost, DateTime.Today);

            // Assert
            Assert.AreEqual("Cost cannot be negative.", msg);
        }

        /// <summary>
        /// Assign job if unassigned.
        /// </summary>
        [TestMethod]
        public void AssignJob_UnassignedJob_AssignsSuccessfully()
        {
            // Arrange
            Contractor c = new Contractor("Ana", "Silva", DateTime.Now, 50);
            Job j = new Job("Build", DateTime.Today, 200);
            system.AddContractor(c);
            system.AddJob(j);

            // Act
            string msg = system.VerifyInputs_AssignJob(j, c);
            bool result = system.AssignJob(j, c);

            // Assert
            Assert.IsNull(msg);
            Assert.IsTrue(result);
            Assert.AreEqual(c, j.ContractorAssigned);
        }

        /// <summary>
        /// Cannot assign already assigned job.
        /// </summary>
        [TestMethod]
        public void AssignJob_AlreadyAssigned_ReturnsError()
        {
            // Arrange
            Contractor c1 = new Contractor("Paul", "Jones", DateTime.Now, 60);
            Contractor c2 = new Contractor("Mike", "Tyson", DateTime.Now, 60);
            Job j = new Job("Clean", DateTime.Today, 100);
            system.AddContractor(c1);
            system.AddContractor(c2);
            system.AddJob(j);
            system.AssignJob(j, c1);

            // Act
            string msg = system.VerifyInputs_AssignJob(j, c2);

            // Assert
            Assert.AreEqual("This job is already assigned.", msg);
        }

        /// <summary>
        /// Complete job clears assignment.
        /// </summary>
        [TestMethod]
        public void CompleteJob_ValidJob_CompletesSuccessfully()
        {
            // Arrange
            Contractor c = new Contractor("Anna", "Blue", DateTime.Now, 70);
            Job j = new Job("Wash", DateTime.Today, 80);
            system.AddContractor(c);
            system.AddJob(j);
            system.AssignJob(j, c);

            // Act
            string msg = system.VerifyInputs_CompleteJob(j);
            system.CompleteJob(j);

            // Assert
            Assert.IsNull(msg);
            Assert.IsTrue(j.Completed);
            Assert.IsNull(j.ContractorAssigned);
        }

        /// <summary>
        /// Null job input returns error.
        /// </summary>
        [TestMethod]
        public void CompleteJob_NullJob_ReturnsError()
        {
            // Arrange
            Job? j = null;

            // Act
            string msg = system.VerifyInputs_CompleteJob(j);

            // Assert
            Assert.AreEqual("Please select a job to complete.", msg);
        }

        /// <summary>
        /// Get all jobs.
        /// </summary>
        [TestMethod]
        public void GetAllJobs_ReturnsAllJobs()
        {
            // Arrange
            system.AddJob(new Job("Fix", DateTime.Today, 150));

            // Act
            var jobs = system.GetAllJobs();

            // Assert
            Assert.AreEqual(1, jobs.Count);
        }

        /// <summary>
        /// Get only unassigned jobs.
        /// </summary>
        [TestMethod]
        public void GetUnassignedJobs_OneAssigned_OneReturned()
        {
            // Arrange
            Job j1 = new Job("One", DateTime.Today, 100);
            Job j2 = new Job("Two", DateTime.Today, 200);
            Contractor c = new Contractor("Gary", "Lopez", DateTime.Now, 90);
            system.AddJob(j1);
            system.AddJob(j2);
            system.AddContractor(c);
            system.AssignJob(j2, c);

            // Act
            var result = system.GetUnassignedJobs();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(j1, result[0]);
        }

        /// <summary>
        /// Valid cost range filters jobs correctly.
        /// </summary>
        [TestMethod]
        public void GetJobsByCost_ValidRange_FiltersCorrectly()
        {
            // Arrange
            Job j1 = new Job("J1", DateTime.Today, 50);
            Job j2 = new Job("J2", DateTime.Today, 150);
            system.AddJob(j1);
            system.AddJob(j2);

            // Act
            string msg = system.VerifyInputs_FilterByCost("100", "200", out decimal? min, out decimal? max);
            var result = system.GetJobsByCost(min, max);

            // Assert
            Assert.IsNull(msg);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(j2, result[0]);
        }

        /// <summary>
        /// Empty cost fields return error message.
        /// </summary>
        [TestMethod]
        public void GetJobsByCost_EmptyInputs_ReturnsError()
        {
            // Act
            string msg = system.VerifyInputs_FilterByCost("", "", out _, out _);

            // Assert
            Assert.AreEqual("Please enter at least one valid cost.", msg);
        }
    }
}

