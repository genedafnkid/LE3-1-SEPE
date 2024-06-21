CREATE PROCEDURE [dbo].[Post_Insert]
	@userId int,
	@title nvarchar(50),
	@body text,
	@dateCreated datetime2
AS
begin	
	INSERT INTO dbo.Posts (UserId, Title, Body, DateCreated) VALUES (@userId, @title, @body, @dateCreated)
end