﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Wyam.Common.Documents;
using Wyam.Common.Meta;
using Wyam.Common.Pipelines;

namespace Wyam.Modules.Git.Tests
{
    [TestFixture]
    public class GitContributorsFixture
    {
        [Test]
        public void GetAllContributorsFromInputPath()
        {
            // Given
            IExecutionContext context = Substitute.For<IExecutionContext>();
            context.InputFolder.Returns(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)))); // Ascend from \bin\Debug
            context.GetNewDocument(Arg.Any<IEnumerable<MetadataItem>>()).Returns(getNewDocumentCallInfo =>
            {
                IDocument newDocument = Substitute.For<IDocument>();
                newDocument.GetEnumerator()
                    .Returns(getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).Select(x => (KeyValuePair<string, object>)x).GetEnumerator());
                newDocument.String(Arg.Any<string>())
                    .Returns(stringCallInfo => (string)getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == stringCallInfo.ArgAt<string>(0)).Value);
                return newDocument;
            });
            IDocument document = Substitute.For<IDocument>();
            GitContributors gitContributors = new GitContributors();

            // When
            List<IDocument> results = gitContributors.Execute(new[] { document }, context).ToList();  // Make sure to materialize the result list

            // Then
            Assert.That(results.Count, Is.GreaterThan(4));
            Dictionary<string, object> result = results
                .Single(x => x.String(GitKeys.ContributorEmail) == "dave@daveaglick.com" && x.String(GitKeys.ContributorName) == "Dave Glick")
                .ToDictionary(x => x.Key, x => x.Value);
            List<Dictionary<string, object>> commits = ((IEnumerable<IDocument>)result[GitKeys.Commits])
                .Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
            Assert.That(commits.Count(), Is.GreaterThan(200));
            Assert.IsTrue(commits.All(x => (string)x[GitKeys.CommitterEmail] == "dave@daveaglick.com" || (string)x[GitKeys.AuthorEmail] == "dave@daveaglick.com"));
        }

        [Test]
        public void GetCommittersFromInputPath()
        {
            // Given
            IExecutionContext context = Substitute.For<IExecutionContext>();
            context.InputFolder.Returns(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)))); // Ascend from \bin\Debug
            context.GetNewDocument(Arg.Any<IEnumerable<MetadataItem>>()).Returns(getNewDocumentCallInfo =>
            {
                IDocument newDocument = Substitute.For<IDocument>();
                newDocument.GetEnumerator()
                    .Returns(getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).Select(x => (KeyValuePair<string, object>)x).GetEnumerator());
                newDocument.String(Arg.Any<string>())
                    .Returns(stringCallInfo => (string)getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == stringCallInfo.ArgAt<string>(0)).Value);
                return newDocument;
            });
            IDocument document = Substitute.For<IDocument>();
            GitContributors gitContributors = new GitContributors().WithAuthors(false);

            // When
            List<IDocument> results = gitContributors.Execute(new[] { document }, context).ToList();  // Make sure to materialize the result list

            // Then
            Assert.That(results.Count, Is.GreaterThan(4));
            Dictionary<string, object> result = results
                .Single(x => x.String(GitKeys.ContributorEmail) == "dave@daveaglick.com" && x.String(GitKeys.ContributorName) == "Dave Glick")
                .ToDictionary(x => x.Key, x => x.Value);
            List<Dictionary<string, object>> commits = ((IEnumerable<IDocument>)result[GitKeys.Commits])
                .Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
            Assert.That(commits.Count, Is.GreaterThan(200));
            Assert.IsTrue(commits.All(x => (string)x[GitKeys.CommitterEmail] == "dave@daveaglick.com"));
        }

        [Test]
        public void GetAuthorsFromInputPath()
        {
            // Given
            IExecutionContext context = Substitute.For<IExecutionContext>();
            context.InputFolder.Returns(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)))); // Ascend from \bin\Debug
            context.GetNewDocument(Arg.Any<IEnumerable<MetadataItem>>()).Returns(getNewDocumentCallInfo =>
            {
                IDocument newDocument = Substitute.For<IDocument>();
                newDocument.GetEnumerator()
                    .Returns(getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).Select(x => (KeyValuePair<string, object>)x).GetEnumerator());
                newDocument.String(Arg.Any<string>())
                    .Returns(stringCallInfo => (string)getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == stringCallInfo.ArgAt<string>(0)).Value);
                return newDocument;
            });
            IDocument document = Substitute.For<IDocument>();
            GitContributors gitContributors = new GitContributors().WithCommitters(false);

            // When
            List<IDocument> results = gitContributors.Execute(new[] { document }, context).ToList();  // Make sure to materialize the result list

            // Then
            Assert.That(results.Count, Is.GreaterThan(4));
            Dictionary<string, object> result = results
                .Single(x => x.String(GitKeys.ContributorEmail) == "dave@daveaglick.com" && x.String(GitKeys.ContributorName) == "Dave Glick")
                .ToDictionary(x => x.Key, x => x.Value);
            List<Dictionary<string, object>> commits = ((IEnumerable<IDocument>)result[GitKeys.Commits])
                .Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
            Assert.That(commits.Count, Is.GreaterThan(200));
            Assert.IsTrue(commits.All(x => (string)x[GitKeys.AuthorEmail] == "dave@daveaglick.com"));
        }

        [Test]
        public void GetCommittersForInputDocument()
        {
            // Given
            IExecutionContext context = Substitute.For<IExecutionContext>();
            context.InputFolder.Returns(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory)))); // Ascend from \bin\Debug
            context.GetNewDocument(Arg.Any<IEnumerable<MetadataItem>>()).Returns(getNewDocumentCallInfo =>
            {
                IDocument newDocument = Substitute.For<IDocument>();
                newDocument.GetEnumerator()
                    .Returns(getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).Select(x => (KeyValuePair<string, object>)x).GetEnumerator());
                newDocument.String(Arg.Any<string>())
                    .Returns(stringCallInfo => (string)getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == stringCallInfo.ArgAt<string>(0)).Value);
                newDocument.Get<IReadOnlyDictionary<string, string>>(Arg.Any<string>())
                    .Returns(getCallInfo => (IReadOnlyDictionary<string, string>)getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == getCallInfo.ArgAt<string>(0)).Value);
                newDocument.Get<IReadOnlyList<IDocument>>(Arg.Any<string>())
                    .Returns(getCallInfo => (IReadOnlyList<IDocument>)getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == getCallInfo.ArgAt<string>(0)).Value);
                newDocument[Arg.Any<string>()]
                    .Returns(getCallInfo => getNewDocumentCallInfo.ArgAt<IEnumerable<MetadataItem>>(0).First(x => x.Key == getCallInfo.ArgAt<string>(0)).Value);
                return newDocument;
            });
            IDocument document = Substitute.For<IDocument>();
            document.Source.Returns(
                Path.Combine(
                    Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory))),
                    "Wyam.Core\\IModule.cs"));  // Use file that no longer exists so commit count is stable
            document.Clone(Arg.Any<IEnumerable<MetadataItem>>()).Returns(x =>
            {
                IDocument newDocument = Substitute.For<IDocument>();
                newDocument.GetEnumerator().Returns(x.ArgAt<IEnumerable<MetadataItem>>(0).Select(y => (KeyValuePair<string, object>)y).GetEnumerator());
                return newDocument;
            });
            GitContributors gitContributors = new GitContributors().ForEachInputDocument();

            // When
            List<IDocument> results = gitContributors.Execute(new[] { document }, context).ToList();  // Make sure to materialize the result list

            // Then
            Assert.AreEqual(1, results.Count);
            List<IDocument> contributors =
                ((IEnumerable<IDocument>)results[0].First(x => x.Key == GitKeys.Contributors).Value).ToList();
            Dictionary<string, object> contributor = contributors
                .Single(x => x.String(GitKeys.ContributorEmail) == "dave@daveaglick.com" && x.String(GitKeys.ContributorName) == "Dave Glick")
                .ToDictionary(x => x.Key, x => x.Value);
            List<Dictionary<string, object>> commits = ((IEnumerable<IDocument>)contributor[GitKeys.Commits])
                .Select(x => x.ToDictionary(y => y.Key, y => y.Value)).ToList();
            Assert.That(commits.Count, Is.LessThan(10));
            Assert.IsTrue(commits.All(x => (string)x[GitKeys.CommitterEmail] == "dave@daveaglick.com" || (string)x[GitKeys.AuthorEmail] == "dave@daveaglick.com"));
        }
    }
}
