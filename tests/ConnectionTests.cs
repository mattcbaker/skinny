using System;
using Xunit;

namespace Skinny
{
    public class when_executing_a_command_that_does_not_alter_rows : IDisposable
    {
        public when_executing_a_command_that_does_not_alter_rows()
        {
            var connection = new Connection(Settings.ConnectionString);
            var command = "CREATE TABLE skinny_testing";

            actual = connection.Command(command);
        }
        public void Dispose()
        {
            // cleanup table using ngpsql
        }

        [Fact]
        public void should_return_negative_one() => Assert.Equal(-1, actual);

        static int actual;
    }
}