namespace DotNet.Testcontainers.Tests.Unit
{
  using DotNet.Testcontainers.Core.Images;
  using DotNet.Testcontainers.Tests.Fixtures;
  using Xunit;

  public class TestcontainersParserTest
  {
    public class ParseDockerImage
    {
      [Theory]
      [ClassData(typeof(ParseDockerImageFixture))]
      public void WhenImageNameGetsAssigend(ParseDockerImageFixtureSerializable serializable, string fullName)
      {
        var expected = serializable.Image;

        // Given
        var dockerImage = new TestcontainersImage();

        // When
        dockerImage.Image = fullName;

        // Then
        Assert.Equal(expected.Repository, dockerImage.Repository);
        Assert.Equal(expected.Name, dockerImage.Name);
        Assert.Equal(expected.Tag, dockerImage.Tag);
      }
    }
  }
}