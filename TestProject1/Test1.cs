
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.DataCollection;

namespace TestProject1
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void AddContractor_ValidInput_ContractorIsAdded()
        {
            RecruitmentSystem recruitmentSystem = new RecruitmentSystem();
            Contractor contractor = new("MARIA","SILVA",new DateTime(2025/11/02), 35.50m);
            recruitmentSystem.AddContractor(contractor);
            Assert.IsTrue(recruitmentSystem.GetAllContractors().Contains(contractor));


            
        }

    }
}
