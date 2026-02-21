-- Insert 3 BlogPosts per category.
-- Uses the category owner (UserId) as AuthorId. Each post gets a unique placeholder image (Picsum Photos, 800x400).
-- Idempotent: skips (category, post#) if that slug already exists.
-- Run against your ShireBudgeters database.

SET NOCOUNT ON;

WITH Numbers(n) AS (
    SELECT 1 UNION ALL SELECT 2 UNION ALL SELECT 3
),
CategoryPosts AS (
    SELECT
        c.Id AS CategoryId,
        c.Name AS CategoryName,
        c.UserId,
        c.CreatedBy,
        c.ModifiedBy,
        n.n
    FROM Categories c
    CROSS JOIN Numbers n
)
INSERT INTO BlogPosts (
    AuthorId,
    Title,
    Slug,
    ContentBody,
    FeaturedImageUrl,
    PublicationDate,
    IsPublished,
    CategoryId,
    MetaDescription,
    CreatedBy,
    CreatedDate,
    ModifiedBy,
    ModifiedDate
)
SELECT
    cp.UserId,
    CASE cp.n
        WHEN 1 THEN N'Welcome to ' + cp.CategoryName
        WHEN 2 THEN N'Getting Started with ' + cp.CategoryName
        WHEN 3 THEN N'Tips and Ideas for ' + cp.CategoryName
    END,
    'category-' + CAST(cp.CategoryId AS NVARCHAR(20)) + '-post-' + CAST(cp.n AS NVARCHAR(5)),
    CASE cp.n
        WHEN 1 THEN N'<h1>Welcome to ' + cp.CategoryName + N'</h1><p>This is a sample post for the ' + cp.CategoryName + N' category.</p>'
        WHEN 2 THEN N'<h1>Getting Started with ' + cp.CategoryName + N'</h1><p>Learn the basics and how to make the most of ' + cp.CategoryName + N'.</p>'
        WHEN 3 THEN N'<h1>Tips and Ideas for ' + cp.CategoryName + N'</h1><p>Practical advice and inspiration for ' + cp.CategoryName + N'.</p>'
    END,
    'https://picsum.photos/seed/' + CAST(cp.CategoryId * 10 + cp.n AS NVARCHAR(20)) + '/800/400',
    DATEADD(DAY, -cp.n, GETUTCDATE()),
    1,
    cp.CategoryId,
    CASE cp.n
        WHEN 1 THEN N'Discover content about ' + cp.CategoryName + N'.'
        WHEN 2 THEN N'Start exploring ' + cp.CategoryName + N' with this guide.'
        WHEN 3 THEN N'Useful tips and ideas for ' + cp.CategoryName + N'.'
    END,
    cp.CreatedBy,
    GETDATE(),
    cp.ModifiedBy,
    GETDATE()
FROM CategoryPosts cp
WHERE NOT EXISTS (
    SELECT 1
    FROM BlogPosts p
    WHERE p.Slug = 'category-' + CAST(cp.CategoryId AS NVARCHAR(20)) + '-post-' + CAST(cp.n AS NVARCHAR(5))
);

PRINT CAST(@@ROWCOUNT AS NVARCHAR(20)) + N' blog post(s) inserted.';
