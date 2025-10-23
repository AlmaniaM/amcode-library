using AMCode.Common.Util;

namespace AMCode.CommonTestingLibrary.UnitTests.Containers.Models
{
    public class TestContainerResources
    {
        public readonly static string AccessToken = EnvironmentUtil.GetEnvironmentVariable("AMCODE_TEST_DOCKER_PAT");
        public readonly static string RepositoryName = "amcode";
        public readonly static string ImageName = "almania/amcode-common-testing-library-vertica";
        public readonly static string NoDbImageName = "almania/amcode-common-testing-library-no-db";
        public readonly static string ImageTag = "1.0.0";
        public readonly static string NoDbImageTag = "1.0.0";
        public readonly static string UserName = EnvironmentUtil.GetEnvironmentVariable("AMCODE_TEST_DOCKER_USERNAME");
    }
}