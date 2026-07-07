using System.IO;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.Configuration;
using NzbDrone.Core.Languages;
using NzbDrone.Core.Movies;
using NzbDrone.Core.Movies.Translations;
using NzbDrone.Core.Organizer;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.OrganizerTests
{
    [TestFixture]

    public class GetMovieFolderFixture : CoreTest<FileNameBuilder>
    {
        private NamingConfig _namingConfig;

        [SetUp]
        public void Setup()
        {
            _namingConfig = NamingConfig.Default;

            Mocker.GetMock<INamingConfigService>()
                  .Setup(c => c.GetConfig()).Returns(_namingConfig);
        }

        [TestCase("Arrival", 2016, "{Movie Title} ({Release Year})", "Arrival (2016)")]
        [TestCase("The Big Short", 2015, "{Movie TitleThe} ({Release Year})", "Big Short, The (2015)")]
        [TestCase("The Big Short", 2015, "{Movie Title} ({Release Year})", "The Big Short (2015)")]
        public void should_use_movieFolderFormat_to_build_folder_name(string movieTitle, int year, string format, string expected)
        {
            _namingConfig.MovieFolderFormat = format;

            var movie = new Movie { Title = movieTitle, Year = year };

            Subject.GetMovieFolder(movie).Should().Be(expected);
        }

        [TestCase("{Movie Title} ({Release Year})", "Vaiana (2026)")]
        [TestCase("{Movie TitleThe} ({Release Year})", "Vaiana (2026)")]
        [TestCase("{Movie CleanTitleThe} ({Release Year})", "Vaiana (2026)")]
        public void should_use_movie_info_language_for_movie_folder_title_tokens(string format, string expected)
        {
            _namingConfig.MovieFolderFormat = format;

            Mocker.GetMock<IConfigService>()
                  .SetupGet(c => c.MovieInfoLanguage)
                  .Returns((int)Language.Spanish);

            var movie = new Movie
            {
                MovieMetadata = new MovieMetadata
                {
                    Title = "Moana",
                    OriginalTitle = "Moana",
                    Year = 2026,
                    Translations =
                    {
                        new MovieTranslation
                        {
                            Language = Language.Spanish,
                            Title = "Vaiana"
                        }
                    }
                }
            };

            Subject.GetMovieFolder(movie).Should().Be(expected);
        }

        [TestCase("The Y-Women Collection", "The Y-Women 14", 2005, "{Movie CollectionThe}/{Movie TitleThe} ({Release Year})", "Y-Women Collection, The", "Y-Women 14, The (2005)")]
        [TestCase("A Decade's Worth of Changes", "The First Year", 1980, "{Movie CleanCollectionThe}/{Movie TitleThe} ({Release Year})", "Decades Worth of Changes, A", "First Year, The (1980)")]
        [TestCase(null, "Just a Movie", 1999, "{Movie Title} ({Release Year})", null, "Just a Movie (1999)")]
        [TestCase(null, "Collectionless Slop", 1949, "{Movie CollectionThe}/{Movie TitleThe} ({Release Year})", null, "Collectionless Slop (1949)")]
        public void should_use_movieFolderFormat_and_CollectionFormat_to_build_folder_name(string collectionTitle, string movieTitle, int year, string format, string expectedCollection, string expectedTitle)
        {
            _namingConfig.MovieFolderFormat = format;

            var movie = new Movie
            {
                MovieMetadata = new MovieMetadata
                {
                    CollectionTitle = collectionTitle,
                    Title = movieTitle,
                    Year = year,
                },
            };

            var result = Subject.GetMovieFolder(movie);
            var expected = !string.IsNullOrWhiteSpace(expectedCollection)
                ? Path.Combine(expectedCollection, expectedTitle)
                : expectedTitle;
            result.Should().Be(expected);
        }
    }
}
