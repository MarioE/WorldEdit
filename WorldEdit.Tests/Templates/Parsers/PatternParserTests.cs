using Moq;
using NUnit.Framework;
using WorldEdit.Templates;
using WorldEdit.Templates.Parsers;

namespace WorldEdit.Tests.Templates.Parsers
{
    [TestFixture]
    public class PatternParserTests
    {
        [TestCase("a,")]
        [TestCase("a,6b")]
        [TestCase("*a,b")]
        [TestCase("a,6*")]
        public void Parse_InvalidPattern_ReturnsNull(string s)
        {
            var template = Mock.Of<ITemplate>();
            var template2 = Mock.Of<ITemplate>();
            var parser = Mock.Of<TemplateParser>(p => p.Parse("a") == template && p.Parse("b") == template2);
            var patternParser = new PatternParser(parser);

            Assert.That(patternParser.Parse(s), Is.Null);
        }

        [Test]
        public void Parse_NullS_ThrowsArgumentNullException()
        {
            var parser = Mock.Of<TemplateParser>();
            var patternParser = new PatternParser(parser);

            // ReSharper disable once AssignNullToNotNullAttribute
            Assert.That(() => patternParser.Parse(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Parse_OneEntry()
        {
            var template = Mock.Of<ITemplate>();
            var template2 = Mock.Of<ITemplate>();
            var parser = Mock.Of<TemplateParser>(p => p.Parse("a") == template && p.Parse("b") == template2);
            var patternParser = new PatternParser(parser);

            var pattern = (Pattern)patternParser.Parse("a");

            Assert.That(pattern, Is.Not.Null);
            Assert.That(pattern.Entries[0].Template, Is.EqualTo(template));
        }

        [Test]
        public void Parse_TwoEntries()
        {
            var template = Mock.Of<ITemplate>();
            var template2 = Mock.Of<ITemplate>();
            var parser = Mock.Of<TemplateParser>(p => p.Parse("a") == template && p.Parse("b") == template2);
            var patternParser = new PatternParser(parser);

            var pattern = (Pattern)patternParser.Parse("a,b");

            Assert.That(pattern, Is.Not.Null);
            Assert.That(pattern.Entries, Has.Some.Matches<PatternEntry>(e => e.Template == template));
            Assert.That(pattern.Entries, Has.Some.Matches<PatternEntry>(e => e.Template == template2));
        }

        [Test]
        public void Parse_TwoEntriesWithWeights()
        {
            var template = Mock.Of<ITemplate>();
            var template2 = Mock.Of<ITemplate>();
            var parser = Mock.Of<TemplateParser>(p => p.Parse("a") == template && p.Parse("b") == template2);
            var patternParser = new PatternParser(parser);

            var pattern = (Pattern)patternParser.Parse("5*a,6*b");

            Assert.That(pattern, Is.Not.Null);
            Assert.That(pattern.Entries, Has.Some.Matches<PatternEntry>(e => e.Template == template && e.Weight == 5));
            Assert.That(pattern.Entries, Has.Some.Matches<PatternEntry>(e => e.Template == template2 && e.Weight == 6));
        }
    }
}
