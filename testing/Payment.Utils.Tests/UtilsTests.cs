using Payment.Utils;

namespace Payment.Utils.Test
{
    public class UtilsTests
    {
        [Fact]
        public void GetRequiredEnvironmentVariable_NoEnvironmentVariable_ThrowsArgumentNullException()
        {
            var act = () => Utils.GetRequiredEnvironmentVariable("DeathStarPlans");

            var exception = Assert.Throws<ArgumentNullException>(act);
            Assert.Equal("Value cannot be null. (Parameter 'Environment Variable: DeathStarPlans')", exception.Message);
        }

        [Fact]
        public void GetRequiredEnvironmentVariable_EnvironmentVariableSet_ReturnsValue()
        {
            var key = "DeathStarPlans";
            var value = "Join the dark side we have cookies";
            Environment.SetEnvironmentVariable(key,value);
            var result = Utils.GetRequiredEnvironmentVariable(key);

            Assert.Equal(value, result);
        }
    }
}