using BlogApi.Utils;

namespace BlogApiTests.Utils
{
    public class StringUtilTests
    {
        [Fact]
        public void ToTitleCase_LowerCaseString_ReturnsTitleCase()
        {
            // Arrange
            var lowerCaseString = "lorem ipsum";
            var expectedString = "Lorem Ipsum";

            // Act
            var result = StringUtil.ToTitleCase(lowerCaseString);

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ToTitleCase_UpperCaseString_ReturnsTitleCase()
        {
            // Arrange
            var upperCaseString = "LOREM IPSUM";
            var expectedString = "Lorem Ipsum";

            // Act
            var result = StringUtil.ToTitleCase(upperCaseString);

            // Assert
            Assert.Equal(expectedString, result);
        }

        [Fact]
        public void ToTitleCase_TitleCaseString_ReturnsTitleCase()
        {
            // Arrange
            var titleCaseString = "Lorem Ipsum";
            var expectedString = "Lorem Ipsum";

            // Act
            var result = StringUtil.ToTitleCase(titleCaseString);

            // Assert
            Assert.Equal(expectedString, result);
        }


        [Fact]
        public void HashPlainText_StringInput_ReturnsHashedValue()
        {
            // Arrange
            var testString = "CCS";
            var expected = "utL3L5EtRx+R4g302xTb63DDhkmOMuRv2PQIWlM+Ro4=";
            byte[] salt = { 1, 2, 3 };

            // Act
            var result = StringUtil.HashPlainText(testString, salt);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
