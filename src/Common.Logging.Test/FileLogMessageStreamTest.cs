namespace Common.Logging.Test
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for the <see cref="FileLogMessageStream"/> class.
    /// </summary>
    [TestClass]
    public class FileLogMessageStreamTest
    {
        /// <summary>
        /// Gets or sets the test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Tests basic logging functionality of the <see cref="FileLogMessageStream.Log(LogMessage)"/> method.
        /// </summary>
        [TestMethod]
        public void FileLogMessageStream_LogTest()
        {
            string rootPath = Path.Combine(this.TestContext.TestDeploymentDir, Guid.NewGuid().ToString());
            using (FileLogMessageStream log = FileLogMessageStream.Create(rootPath))
            {
                log.Log(LogMessage.Information("Test"));
            }

            string filePath = Directory.EnumerateFiles(rootPath).FirstOrDefault();
            Assert.IsNotNull(filePath);
            Assert.IsTrue(File.ReadAllText(filePath).Contains("Test"));
        }

        /// <summary>
        /// Tests logging into multiple files in the <see cref="FileLogMessageStream.Log(LogMessage)"/> method.
        /// </summary>
        [TestMethod]
        public void FileLogMessageStream_LogMultipleFilesTest()
        {
            string rootPath = Path.Combine(this.TestContext.TestDeploymentDir, Guid.NewGuid().ToString());
            using (FileLogMessageStream log = FileLogMessageStream.Create(rootPath))
            {
                LogMessage message = LogMessage.Create(LogMessageSeverity.Verbose, "Old.");
                message.Timestamp = DateTime.UtcNow - TimeSpan.FromDays(1.0);
                log.Log(message);
                log.Log(LogMessage.Information("Test"));
            }

            string[] logFiles = Directory.EnumerateFiles(rootPath).ToArray();
            Assert.AreEqual(2, logFiles.Length);
            string[] logContent =
                (from e in logFiles
                 select File.ReadAllText(e)).ToArray();
            Assert.IsTrue(logContent.Any(e => e.Contains("Old")));
            Assert.IsTrue(logContent.Any(e => e.Contains("Test")));
        }
    }
}
