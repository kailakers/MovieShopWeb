select *
from Movie
order by id
for json path, INCLUDE_NULL_VALUES , root('data');

-- Select rows from a Table or View '[TableOrViewName]' in schema '[dbo]'
SELECT * FROM [dbo].[Movie]

GO

  select *
  from MovieGenre
  where MovieId = 204

  SELECT
    mb.Title, mb.Overview, mb.Tagline, mb.BackdropUrl, mb.PosterUrl, mb.ExternalId, mb.Budget,
    mb.Revenue, mb.ReleaseDate, mb.ImdbId, mb.OriginalLanguage, mb.RunTime,
    g.Id as genreId, g.Name

  FROM [MoviesDB].[dbo].[Movies] mb
    JOIN [MoviesDB].dbo.GenreMovies mg on mb.Id = mg.Movie_Id
    JOIN [MoviesDB].dbo.Genres g on mg.Genre_Id = g.Id
  where mb.OriginalTitle not in (SELECT m.title
  FROM [MovieShop].[dbo].[Movie] m)
  order by mb.Popularity desc

  -- Select rows from a Table or View '[TableOrViewName]' in schema '[dbo]'
  SELECT *
  FROM [MoviesDB].dbo.GenreMovies
 GO

  --FOR JSON PATH, Root('Movies'), INCLUDE_NULL_VALUES ;


  DECLARE @result NVARCHAR(max);

  SET @result = (SELECT *
  FROM Movie
  FOR JSON AUTO, ROOT('Data'))

  PRINT @result;