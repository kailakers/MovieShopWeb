USE MovieShop;
SELECT r.MovieId, m.Title, m.PosterUrl, AVG(r.Rating) AS Rating
FROM Movie m
    left JOIN Review r ON m.Id = r.MovieId
GROUP BY r.MovieId, m.Title, m.PosterUrl
order by AVG(r.Rating) desc


-- EF 3 Generated SQL

SELECT TOP(25)
    [r].[MovieId] AS [Id], [m].[PosterUrl], [m].[Title], [m].[BackdropUrl], AVG([r].[Rating]) AS [Rating]
FROM [Review] AS [r]
    INNER JOIN [Movie] AS [m] ON [r].[MovieId] = [m].[Id]
GROUP BY [r].[MovieId], [m].[PosterUrl], [m].[Title], [m].[BackdropUrl]
ORDER BY AVG([r].[Rating]) DESC


SELECT TOP(25)
    [r].[MovieId] AS [Id], AVG([r].[Rating]) AS [Rating]
FROM [Review] AS [r]
GROUP BY [r].[MovieId]
ORDER BY AVG([r].[Rating]) DESC

SELECT r.MovieId,
    AVG(r.Rating)
FROM [Review] r
GROUP BY r.MovieId
ORDER BY AVG(r.Rating) DESC offset 0 rows
		FETCH NEXT 10 rows ONLY;


SELECT u.Id  
	, u.FirstName
	, CAST(rr.averagerating as decimal(4,2))
      --ROUND( rr.averagerating, 2)
	, rr.reviewcount
FROM [User] u
    JOIN (
	SELECT r.UserId
		, COUNT(r.UserId) AS reviewcount
		, AVG(r.Rating) AS averagerating
    FROM Review r
    GROUP BY r.UserId
	) AS RR ON u.Id = rr.UserId
ORDER BY rr.averagerating desc

select u.Id  
	, u.FirstName 
	, count(r.UserId) as MoviesReviewed
	, AVG(r.Rating) AverageRating
FROM [User] u left join Review r
    on u.Id = r.UserId
group by u.Id, u.FirstName
order by AverageRating desc


-- EF 3 Favorite Exixts
SELECT CASE
          WHEN EXISTS (
              SELECT 1
    FROM [Favorite] AS [f]
    WHERE ([f].[MovieId] = 14) AND ([f].[UserId] = 1)) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
      END

-- Get Purchases by User
SELECT [p].[Id], [p].[MovieId], [p].[PurchaseDateTime], [p].[PurchaseNumber], [p].[TotalPrice], [p].[UserId], [m].[Id], [m].[BackdropUrl], [m].[Budget], [m].[CreatedBy], [m].[CreatedDate], [m].[ImdbUrl], [m].[OriginalLanguage], [m].[Overview], [m].[PosterUrl], [m].[Price], [m].[ReleaseDate], [m].[Revenue], [m].[RunTime], [m].[Tagline], [m].[Title], [m].[TmdbUrl], [m].[UpdatedBy], [m].[UpdatedDate]
FROM [Purchase] AS [p]
    INNER JOIN [Movie] AS [m] ON [p].[MovieId] = [m].[Id]
WHERE [p].[UserId] = 1

-- Get Reviews by user by EF

SELECT [r].[MovieId], [r].[UserId], [r].[Rating], [r].[ReviewText], [m].[Id], [m].[BackdropUrl], [m].[Budget], [m].[CreatedBy], [m].[CreatedDate], [m].[ImdbUrl], [m].[OriginalLanguage], [m].[Overview], [m].[PosterUrl], [m].[Price], [m].[ReleaseDate], [m].[Revenue], [m].[RunTime], [m].[Tagline], [m].[Title], [m].[TmdbUrl], [m].[UpdatedBy], [m].[UpdatedDate]
FROM [Review] AS [r]
    INNER JOIN [Movie] AS [m] ON [r].[MovieId] = [m].[Id]
WHERE [r].[UserId] = 1

SELECT [r].[MovieId], [r].[UserId], [r].[Rating], [r].[ReviewText]
FROM [Review] AS [r]
WHERE ([r].[UserId] = 1) AND ([r].[MovieId] = 3)

select @@VERSION

select Id, Title
from Movie
order by id
for json path, INCLUDE_NULL_VALUES , root('data')
    select *
    from MovieGenre
    where MovieId = 204

    SELECT mb.*
    FROM [MoviesDB].[dbo].[Movies] mb
    where mb.OriginalTitle not in (SELECT m.title
    FROM [MovieShop].[dbo].[Movie] m)
    order by mb.VoteCount desc
    FOR JSON PATH, Root('Movies'), INCLUDE_NULL_VALUES
        ;

        --  delete from MovieGenre where movieid = 203
        --  delete from Movie where id =203
        -- 
        SELECT AVG([r].[Rating])
        FROM [Review] AS [r]
        WHERE [r].[MovieId] = 204


        SELECT [t].[Id], [t].[BackdropUrl], [t].[Budget], [t].[CreatedBy], [t].[CreatedDate], [t].[ImdbUrl], [t].[OriginalLanguage], [t].[Overview], [t].[PosterUrl], [t].[Price], [t].[ReleaseDate], [t].[Revenue], [t].[RunTime], [t].[Tagline], [t].[Title], [t].[TmdbUrl], [t].[UpdatedBy], [t].[UpdatedDate], [t0].[CastId], [t0].[MovieId], [t0].[Character], [t0].[Id], [t0].[Gender], [t0].[Name], [t0].[ProfilePath], [t0].[TmdbUrl]
        FROM (
          SELECT TOP(1)
                [m].[Id], [m].[BackdropUrl], [m].[Budget], [m].[CreatedBy], [m].[CreatedDate], [m].[ImdbUrl], [m].[OriginalLanguage], [m].[Overview], [m].[PosterUrl], [m].[Price], [m].[ReleaseDate], [m].[Revenue], [m].[RunTime], [m].[Tagline], [m].[Title], [m].[TmdbUrl], [m].[UpdatedBy], [m].[UpdatedDate]
            FROM [Movie] AS [m]
            WHERE [m].[Id] = 204
      ) AS [t]
            LEFT JOIN (
          SELECT [m0].[CastId], [m0].[MovieId], [m0].[Character], [c].[Id], [c].[Gender], [c].[Name], [c].[ProfilePath], [c].[TmdbUrl]
            FROM [MovieCast] AS [m0]
                INNER JOIN [Cast] AS [c] ON [m0].[CastId] = [c].[Id]
      ) AS [t0] ON [t].[Id] = [t0].[MovieId]
        ORDER BY [t].[Id], [t0].[CastId], [t0].[MovieId], [t0].[Character], [t0].[Id]


        SELECT [t].[Id], [t].[BackdropUrl], [t].[Budget], [t].[CreatedBy], [t].[CreatedDate], [t].[ImdbUrl], [t].[OriginalLanguage], [t].[Overview], [t].[PosterUrl], [t].[Price], [t].[ReleaseDate], [t].[Revenue], [t].[RunTime], [t].[Tagline], [t].[Title], [t].[TmdbUrl], [t].[UpdatedBy], [t].[UpdatedDate], [t0].[CastId], [t0].[MovieId], [t0].[Character], [t0].[Id], [t0].[Gender], [t0].[Name], [t0].[ProfilePath], [t0].[TmdbUrl], [r].[MovieId], [r].[UserId], [r].[Rating], [r].[ReviewText]
        FROM (
          SELECT TOP(1)
                [m].[Id], [m].[BackdropUrl], [m].[Budget], [m].[CreatedBy], [m].[CreatedDate], [m].[ImdbUrl], [m].[OriginalLanguage], [m].[Overview], [m].[PosterUrl], [m].[Price], [m].[ReleaseDate], [m].[Revenue], [m].[RunTime], [m].[Tagline], [m].[Title], [m].[TmdbUrl], [m].[UpdatedBy], [m].[UpdatedDate]
            FROM [Movie] AS [m]
            WHERE [m].[Id] = 204
      ) AS [t]
            LEFT JOIN (
          SELECT [m0].[CastId], [m0].[MovieId], [m0].[Character], [c].[Id], [c].[Gender], [c].[Name], [c].[ProfilePath], [c].[TmdbUrl]
            FROM [MovieCast] AS [m0]
                INNER JOIN [Cast] AS [c] ON [m0].[CastId] = [c].[Id]
      ) AS [t0] ON [t].[Id] = [t0].[MovieId]
            LEFT JOIN [Review] AS [r] ON [t].[Id] = [r].[MovieId]
        ORDER BY [t].[Id], [t0].[CastId], [t0].[MovieId], [t0].[Character], [t0].[Id], [r].[MovieId], [r].[UserId]




        SELECT [t].[Id], [t].[BackdropUrl], [t].[Budget], [t].[CreatedBy], [t].[CreatedDate], [t].[ImdbUrl], [t].[OriginalLanguage], [t].[Overview], [t].[PosterUrl], [t].[Price], [t].[ReleaseDate], [t].[Revenue], [t].[RunTime], [t].[Tagline], [t].[Title], [t].[TmdbUrl], [t].[UpdatedBy], [t].[UpdatedDate], [t0].[CastId], [t0].[MovieId], [t0].[Character], [t0].[Id], [t0].[Gender], [t0].[Name], [t0].[ProfilePath], [t0].[TmdbUrl], [t1].[MovieId], [t1].[GenreId], [t1].[Id], [t1].[Name], [r].[MovieId], [r].[UserId], [r].[Rating], [r].[ReviewText]
        FROM (
          SELECT TOP(1)
                [m].[Id], [m].[BackdropUrl], [m].[Budget], [m].[CreatedBy], [m].[CreatedDate], [m].[ImdbUrl], [m].[OriginalLanguage], [m].[Overview], [m].[PosterUrl], [m].[Price], [m].[ReleaseDate], [m].[Revenue], [m].[RunTime], [m].[Tagline], [m].[Title], [m].[TmdbUrl], [m].[UpdatedBy], [m].[UpdatedDate]
            FROM [Movie] AS [m]
            WHERE [m].[Id] = 12
      ) AS [t]
            LEFT JOIN (
          SELECT [m0].[CastId], [m0].[MovieId], [m0].[Character], [c].[Id], [c].[Gender], [c].[Name], [c].[ProfilePath], [c].[TmdbUrl]
            FROM [MovieCast] AS [m0]
                INNER JOIN [Cast] AS [c] ON [m0].[CastId] = [c].[Id]
      ) AS [t0] ON [t].[Id] = [t0].[MovieId]
            LEFT JOIN (
          SELECT [m1].[MovieId], [m1].[GenreId], [g].[Id], [g].[Name]
            FROM [MovieGenre] AS [m1]
                INNER JOIN [Genre] AS [g] ON [m1].[GenreId] = [g].[Id]
      ) AS [t1] ON [t].[Id] = [t1].[MovieId]
            LEFT JOIN [Review] AS [r] ON [t].[Id] = [r].[MovieId]
        ORDER BY [t].[Id], [t0].[CastId], [t0].[MovieId], [t0].[Character], [t0].[Id], [t1].[MovieId], [t1].[GenreId], [t1].[Id], [r].[MovieId], [r].[UserId]


        SELECT [r].[MovieId], [r].[UserId], [r].[Rating], [r].[ReviewText], [u].[Id], [u].[FirstName], [u].[LastName]
        FROM [Review] AS [r]
            INNER JOIN [User] AS [u] ON [r].[UserId] = [u].[Id]
        WHERE [r].[MovieId] =1


        SELECT *
        from Purchase

        -- Select rows from a Table or View '[TableOrViewName]' in schema '[dbo]'
        SELECT p.MovieId, m.Title, COUNT(*) PurchaseCount
        FROM [dbo].[Purchase] p
            JOIN Movie m on p.MovieId = m.Id
        GROUP by p.MovieId, m.Title
        ORDER by COUNT(*) DESC

        -- Insert rows into table 'TableName' in schema '[dbo]'
        INSERT INTO [dbo].[Purchase]
            ( -- Columns to insert data into
            [UserId], [MovieId], [PurchaseNumber], [TotalPrice], [PurchaseDateTime]
            )
        VALUES
            ( 28, 1, NEWID(), 9.9, GETDATE()),
            ( 3, 1, NEWID(), 9.9, GETDATE()),
            ( 4, 1, NEWID(), 9.9, GETDATE()),
            ( 5, 1, NEWID(), 9.9, GETDATE()),
            ( 6, 1, NEWID(), 9.9, GETDATE()),
            ( 7, 1, NEWID(), 9.9, GETDATE()),
            ( 8, 1, NEWID(), 9.9, GETDATE()),
            ( 9, 1, NEWID(), 9.9, GETDATE()),
            ( 10, 1, NEWID(), 9.9, GETDATE()),
            ( 11, 1, NEWID(), 9.9, GETDATE()),
            ( 32, 1, NEWID(), 9.9, GETDATE()),
            ( 24, 1, NEWID(), 9.9, GETDATE()),
            ( 27, 1, NEWID(), 9.9, GETDATE()),
            ( 25, 1, NEWID(), 9.9, GETDATE())

-- Add more rows here
GO